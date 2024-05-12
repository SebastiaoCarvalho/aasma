using System.Collections.Generic;

public class RoomMap {
    private static List<List<int>> roomConnections = new List<List<int>>
        {
            new List<int> { 2, 5, 6 },
            new List<int> { 1, 3 },
            new List<int> { 4, 7, 17 },
            new List<int> { 3, 5, 14 },
            new List<int> { 1, 4 },
            new List<int> { 1, 10, 14 },
            new List<int> { 3, 8, 15, 16 },
            new List<int> { 7, 9, 15, 17 },
            new List<int> { 8, 11, 13, 16 },
            new List<int> { 6, 11, 12 },
            new List<int> { 9, 10 },
            new List<int> { 10, 13 },
            new List<int> { 9, 12, 18 },
            new List<int> { 4, 6 },
            new List<int> { 7, 8, 16 },
            new List<int> { 7, 9, 15 },
            new List<int> { 3, 8 },
            new List<int> { 18 } // exit
        };

    public static List<int> GetRoomConnections(int room) {
        return roomConnections[room - 1];
    }

}