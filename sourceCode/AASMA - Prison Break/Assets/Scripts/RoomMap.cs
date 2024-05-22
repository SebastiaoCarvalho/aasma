using System.Collections.Generic;
using UnityEngine;

public class RoomMap {
    private static List<List<int>> roomConnections = new List<List<int>>
        {
            new List<int> { 2, 5, 6 }, // 1
            new List<int> { 1, 3 }, // 2
            new List<int> { 4, 7, 17 }, // 3
            new List<int> { 3, 5, 14 }, // 4
            new List<int> { 1, 4 }, // 5
            new List<int> { 1, 10, 14 }, // 6
            new List<int> { 3, 8, 15, 16 }, // 7
            new List<int> { 7, 9, 15, 17 }, // 8
            new List<int> { 8, 11, 13, 16 }, // 9
            new List<int> { 6, 11, 12 }, // 10
            new List<int> { 9, 10 }, // 11
            new List<int> { 10, 13 }, // 12
            new List<int> { 9, 12, 18 }, // 13
            new List<int> { 4, 6 }, // 14
            new List<int> { 7, 8, 16 }, // 15
            new List<int> { 7, 9, 15 }, // 16
            new List<int> { 3, 8 }, // 17
            new List<int> { 18 } // exit
        };

    public static List<int> GetRoomConnections(int room) {
        return roomConnections[room - 1];
    }

}