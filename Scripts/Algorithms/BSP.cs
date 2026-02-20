using Godot;
using System;
using System.Collections.Generic;

public static class BSP
{
    public struct Result
    {
        public BspNode Root;
        public List<Room> Rooms;
        public List<Corridor> Corridors;
    }

    public static Result Generate(
        int chunkWidth,
        int chunkHeight,
        int minDepth,
        int maxDepth,
        int minLeafSize,   
        float splitChance,
        int seed,
        int roomMarginMin,
        int roomMarginMax
    )
    {
        var rng = new Random(seed);

        // BuildBSP(region, depth)
        var rootRegion = new Rect2I(0, 0, chunkWidth, chunkHeight);
        var root = BuildBSP(rootRegion, depth: 0, minDepth, maxDepth, minLeafSize, splitChance, rng);

        // PlaceRooms(tree)
        var rooms = new List<Room>();
        PlaceRooms(root, rooms, rng, roomMarginMin, roomMarginMax);

        // ConnectRegions(node)
        var corridors = new List<Corridor>();
        ConnectRegions(root, corridors, rng);

        return new Result { Root = root, Rooms = rooms, Corridors = corridors };
    }

    private static BspNode BuildBSP(
        Rect2I region,
        int depth,
        int minDepth,
        int maxDepth,
        int minLeafSize,
        float splitChance,
        Random rng
    )
    {
        var node = new BspNode(region);

        bool regionTooSmall =
            region.Size.X < minLeafSize * 2 || region.Size.Y < minLeafSize * 2;

        bool forceStop = depth >= maxDepth || regionTooSmall;

        // 1. if depth >= maxDepth or region too small: mark leaf, return node
        if (forceStop)
            return node;

        if (depth >= minDepth)
        {
            if (rng.NextDouble() > splitChance)
                return node; // stop splitting early based on chance
        }

        // 2. choose axis (horizontal or vertical)
        bool splitVertical = region.Size.X >= region.Size.Y;

        // 3. choose split position so both halves meet min size
        if (splitVertical)
        {
            int minSplit = region.Position.X + minLeafSize;
            int maxSplit = region.Position.X + region.Size.X - minLeafSize;

            if (maxSplit <= minSplit)
                return node; // can’t split safely

            int splitX = rng.Next(minSplit, maxSplit);

            // leftRegion and rightRegion
            var leftRegion = new Rect2I(
                region.Position.X,
                region.Position.Y,
                splitX - region.Position.X,
                region.Size.Y
            );

            var rightRegion = new Rect2I(
                splitX,
                region.Position.Y,
                (region.Position.X + region.Size.X) - splitX,
                region.Size.Y
            );

            // 4. BuildBSP left and right

            node.Left = BuildBSP(leftRegion, depth + 1, minDepth, maxDepth, minLeafSize, splitChance, rng);
            node.Right = BuildBSP(rightRegion, depth + 1, minDepth, maxDepth, minLeafSize, splitChance, rng);
        }
        else
        {
            int minSplit = region.Position.Y + minLeafSize;
            int maxSplit = region.Position.Y + region.Size.Y - minLeafSize;

            if (maxSplit <= minSplit)
                return node;

            int splitY = rng.Next(minSplit, maxSplit);

            var leftRegion = new Rect2I(
                region.Position.X,
                region.Position.Y,
                region.Size.X,
                splitY - region.Position.Y
            );

            var rightRegion = new Rect2I(
                region.Position.X,
                splitY,
                region.Size.X,
                (region.Position.Y + region.Size.Y) - splitY
            );

            node.Left = BuildBSP(leftRegion, depth + 1, minDepth, maxDepth, minLeafSize, splitChance, rng);
            node.Right = BuildBSP(rightRegion, depth + 1, minDepth, maxDepth, minLeafSize, splitChance, rng);
        }

        return node;
    }

    private static void PlaceRooms(
        BspNode node,
        List<Room> rooms,
        Random rng,
        int marginMin,
        int marginMax
    )
    {
        if (node == null) return;

        if (node.IsLeaf)
        {
            Rect2I cell = node.Region;

            int margin = rng.Next(marginMin, marginMax + 1);

            int maxRoomW = cell.Size.X - 2 * margin;
            int maxRoomH = cell.Size.Y - 2 * margin;

            // If we can't fit a room, just skip placing one
            if (maxRoomW < 3 || maxRoomH < 3)
                return;

            // Randomize room size so it doesn't scream "leaf cell"
            int roomW = rng.Next((int)(maxRoomW * 0.50f), maxRoomW + 1);
            int roomH = rng.Next((int)(maxRoomH * 0.50f), maxRoomH + 1);

            // Randomize position inside the cell (respecting margin)
            int roomX = rng.Next(cell.Position.X + margin, cell.Position.X + cell.Size.X - margin - roomW + 1);
            int roomY = rng.Next(cell.Position.Y + margin, cell.Position.Y + cell.Size.Y - margin - roomH + 1);

            var roomRect = new Rect2I(roomX, roomY, roomW, roomH);
            node.Room = new Room(roomRect);
            rooms.Add(node.Room);
            return;
        }

        // Place rooms in left and right subtrees
        PlaceRooms(node.Left, rooms, rng, marginMin, marginMax);
        PlaceRooms(node.Right, rooms, rng, marginMin, marginMax);
    }


    private static void ConnectRegions(BspNode node, List<Corridor> corridors, Random rng)
    {
        if (node == null) return;
        if (node.IsLeaf) return;

        // 1. Recursively connect left and right subtrees
        ConnectRegions(node.Left, corridors, rng);
        ConnectRegions(node.Right, corridors, rng);

        // leftCenter = center of node.left.region
        // rightCenter = center of node.right.region
        Vector2I a = node.Left.Center;
        Vector2I b = node.Right.Center;

        // draw corridor (line or L-shape) from a to b
        // choose L-shape orientation randomly
        bool horizFirst = rng.Next(0, 2) == 0;
        Vector2I corner = horizFirst
            ? new Vector2I(b.X, a.Y)
            : new Vector2I(a.X, b.Y);

        corridors.Add(new Corridor(a, corner, b));
    }
}