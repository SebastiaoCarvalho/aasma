using System.Collections.Generic;
using UnityEngine;

public class RoomMap {
    private static List<List<int>> roomConnections = new List<List<int>>
        {
            new List<int> { 2, 5 }, // 1
            new List<int> { 1, 3 }, // 2
            new List<int> { 5, 7, 16 }, // 3
            new List<int> { 5, 6 }, // 4
            new List<int> { 1, 3, 4, 6 }, // 5
            new List<int> { 4, 5, 10 }, // 6
            new List<int> { 3, 8, 14, 15 }, // 7
            new List<int> { 7, 9, 14, 16 }, // 8
            new List<int> { 8, 11, 13, 15 }, // 9
            new List<int> { 6, 11, 12 }, // 10
            new List<int> { 9, 10 }, // 11
            new List<int> { 10, 13 }, // 12
            new List<int> { 9, 12, 17 }, // 13
            new List<int> { 7, 8, 15 }, // 14
            new List<int> { 7, 9, 14 }, // 15
            new List<int> { 3, 8 }, // 16
            new List<int> { 17 } // exit
        };

    public static List<int> GetRoomConnections(int room) {
        return roomConnections[room - 1];
    }

}