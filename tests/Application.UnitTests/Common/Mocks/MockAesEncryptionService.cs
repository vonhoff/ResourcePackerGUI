﻿using System.Text;
using Application.UnitTests.Common.Utilities;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace Application.UnitTests.Common.Mocks
{
    internal class MockAesEncryptionService : IAesEncryptionService
    {
        private static readonly Dictionary<int, byte[]> DecryptTable = new()
        {
            {
                392981100,
                new byte[]
                {
                    151, 204, 221, 82, 99, 237, 62, 126, 163, 13, 159, 203, 130, 7, 24, 18, 118, 155, 72, 72, 45, 54,
                    66, 158, 0, 51, 217, 58, 92, 93, 129, 37, 44, 50, 60, 74, 190, 81, 2, 107, 202, 212, 65, 234, 236,
                    151, 110, 92, 249, 147, 11, 151, 250, 175, 214, 175, 91, 36, 144, 248, 55, 31, 220, 78, 160, 105,
                    96, 41, 184, 87, 26, 100, 163, 218, 143, 112, 228, 14, 120, 115, 25, 254, 212, 133, 78, 30, 72, 254,
                    140, 169, 52, 137, 179, 203, 122, 216, 181, 200, 155, 120, 72, 95, 109, 244, 36, 175, 236, 30, 244,
                    185, 215, 60, 157, 31, 52, 58, 240, 204, 57, 121, 175, 242, 38, 137, 97, 28, 28, 198, 149, 149, 64,
                    231, 229, 22, 2, 248, 32, 246, 191, 250, 200, 214, 45, 254, 46, 223, 104, 215, 153, 247, 94, 186,
                    101, 188, 174, 3, 71, 151, 66, 185, 43, 109, 39, 171, 178, 145, 138, 84, 208, 214, 32, 189, 225,
                    155, 133, 38, 134, 170, 217, 49, 147, 158, 138, 228, 154, 243, 72, 3, 150, 232, 66, 198, 144, 186,
                    67, 98, 34, 35, 79, 131, 174, 107, 177, 138, 131, 179, 199, 15, 167, 93, 112, 251, 143, 28, 88, 94,
                    75, 71, 109, 40, 179, 17, 126, 182, 251, 179, 178, 11, 16, 117, 11, 55, 51, 223, 119, 211, 17, 240,
                    220, 220, 153, 140, 239, 103, 216, 123, 5, 134, 243, 149, 133, 83, 34, 97, 42, 211, 23, 71, 26, 160,
                    4, 22, 108, 26, 228, 182, 82, 107, 71, 61, 18, 16, 80, 4, 120, 37, 163, 98, 186, 185, 181, 216
                }
            },
            {
                858989967,
                new byte[]
                {
                    137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 25, 0, 0, 0, 33, 4, 3, 0, 0,
                    0, 222, 238, 29, 144, 0, 0, 0, 18, 80, 76, 84, 69, 0, 0, 0, 85, 59, 25, 108, 72, 20, 148, 109, 39,
                    179, 142, 71, 65, 43, 15, 48, 142, 193, 40, 0, 0, 0, 1, 116, 82, 78, 83, 0, 64, 230, 216, 102, 0, 0,
                    0, 182, 73, 68, 65, 84, 24, 211, 117, 205, 203, 13, 131, 48, 12, 6, 224, 180, 18, 247, 218, 136,
                    123, 227, 100, 128, 66, 24, 0, 129, 7, 32, 72, 222, 127, 149, 26, 39, 84, 81, 213, 58, 151, 124,
                    250, 253, 112, 63, 139, 227, 210, 96, 229, 248, 193, 204, 145, 215, 43, 12, 105, 101, 77, 11, 238,
                    169, 104, 169, 34, 82, 209, 211, 68, 20, 56, 234, 51, 221, 48, 164, 57, 18, 91, 39, 128, 15, 54,
                    200, 47, 213, 1, 216, 171, 54, 222, 78, 101, 15, 154, 77, 41, 37, 83, 0, 101, 168, 234, 112, 68,
                    160, 83, 182, 51, 71, 64, 68, 147, 133, 172, 123, 125, 26, 85, 22, 42, 53, 126, 152, 220, 17, 55,
                    208, 170, 186, 17, 165, 81, 89, 212, 101, 36, 61, 233, 139, 6, 145, 76, 228, 135, 34, 16, 233, 36,
                    131, 148, 49, 24, 118, 231, 68, 97, 34, 235, 169, 133, 83, 171, 254, 75, 251, 127, 249, 86, 7, 180,
                    202, 224, 90, 13, 173, 68, 174, 223, 27, 121, 206, 37, 24, 249, 122, 183, 139, 0, 0, 0, 0, 73, 69,
                    78, 68, 174, 66, 96, 130
                }
            },
            {
                152769788,
                new byte[]
                {
                    137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 36, 0, 0, 0, 36, 4, 3, 0, 0,
                    0, 19, 46, 133, 171, 0, 0, 0, 15, 80, 76, 84, 69, 0, 0, 0, 65, 43, 15, 113, 106, 12, 75, 57, 22,
                    150, 135, 21, 141, 112, 72, 163, 0, 0, 0, 1, 116, 82, 78, 83, 0, 64, 230, 216, 102, 0, 0, 0, 218,
                    73, 68, 65, 84, 40, 207, 133, 209, 209, 169, 196, 32, 16, 133, 97, 19, 44, 32, 71, 27, 136, 227, 54,
                    96, 166, 1, 35, 246, 95, 211, 61, 113, 46, 201, 226, 194, 238, 15, 190, 124, 40, 163, 232, 190, 214,
                    123, 157, 196, 55, 221, 231, 77, 249, 112, 83, 159, 155, 124, 254, 69, 158, 75, 52, 206, 148, 39,
                    170, 164, 50, 6, 118, 249, 39, 49, 178, 235, 26, 229, 221, 101, 24, 185, 137, 252, 77, 171, 30, 70,
                    189, 213, 155, 116, 207, 40, 110, 149, 151, 123, 104, 139, 23, 133, 110, 192, 18, 0, 18, 66, 245,
                    93, 198, 148, 222, 112, 117, 17, 133, 32, 44, 129, 45, 8, 189, 145, 214, 172, 236, 128, 229, 229,
                    122, 21, 169, 64, 147, 209, 73, 98, 120, 10, 103, 112, 163, 5, 128, 8, 152, 128, 100, 161, 137, 98,
                    180, 63, 191, 35, 82, 16, 181, 12, 242, 188, 22, 169, 183, 20, 57, 25, 155, 99, 34, 82, 201, 128,
                    22, 206, 30, 196, 39, 160, 81, 192, 147, 70, 236, 12, 114, 98, 20, 73, 86, 120, 65, 205, 110, 90,
                    83, 72, 176, 12, 216, 34, 162, 38, 79, 146, 11, 216, 246, 70, 25, 152, 169, 124, 210, 242, 70, 127,
                    222, 20, 41, 233, 136, 179, 233, 50, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
                }
            },
            {
                550380143,
                new byte[]
                {
                    137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 128, 0, 0, 0, 48, 4, 3, 0, 0,
                    0, 43, 218, 111, 141, 0, 0, 0, 27, 80, 76, 84, 69, 112, 176, 208, 192, 224, 224, 232, 240, 248, 0,
                    96, 184, 16, 80, 112, 64, 128, 160, 168, 208, 208, 0, 0, 0, 144, 192, 192, 121, 129, 121, 234, 0, 0,
                    3, 116, 73, 68, 65, 84, 72, 199, 133, 149, 77, 79, 227, 48, 16, 134, 13, 2, 206, 56, 139, 114, 119,
                    14, 220, 193, 171, 138, 99, 170, 186, 247, 30, 146, 123, 163, 93, 69, 253, 7, 123, 94, 33, 161, 254,
                    236, 125, 231, 43, 227, 148, 165, 140, 68, 193, 83, 207, 227, 39, 158, 96, 135, 156, 115, 71, 209,
                    116, 75, 164, 108, 241, 179, 32, 118, 58, 176, 92, 208, 56, 140, 227, 190, 100, 0, 164, 106, 114,
                    192, 243, 82, 222, 135, 48, 142, 195, 26, 209, 27, 224, 254, 126, 28, 134, 93, 200, 175, 82, 245,
                    226, 10, 63, 100, 102, 233, 101, 222, 56, 150, 178, 18, 48, 3, 86, 8, 89, 171, 54, 177, 209, 136,
                    191, 164, 94, 231, 137, 68, 37, 224, 6, 80, 40, 225, 85, 1, 121, 74, 26, 173, 213, 59, 1, 18, 107,
                    1, 223, 4, 7, 168, 66, 132, 128, 215, 251, 211, 22, 23, 88, 109, 66, 232, 12, 144, 231, 200, 49,
                    251, 66, 245, 90, 195, 206, 1, 181, 130, 3, 64, 224, 176, 121, 23, 10, 99, 217, 57, 184, 218, 132,
                    96, 143, 118, 56, 232, 156, 30, 100, 217, 224, 5, 115, 160, 196, 222, 122, 113, 70, 100, 139, 55, 5,
                    244, 242, 64, 5, 40, 66, 35, 168, 253, 12, 136, 143, 58, 30, 138, 148, 199, 216, 158, 22, 196, 95,
                    3, 72, 87, 25, 96, 6, 2, 184, 137, 177, 54, 56, 235, 70, 41, 225, 45, 6, 107, 142, 46, 90, 25, 56,
                    192, 12, 172, 30, 14, 32, 40, 96, 167, 0, 93, 244, 147, 1, 102, 63, 154, 1, 230, 91, 204, 243, 249,
                    157, 158, 32, 134, 82, 27, 12, 181, 129, 3, 204, 0, 243, 227, 162, 112, 58, 27, 160, 218, 121, 44,
                    210, 187, 65, 185, 52, 80, 1, 87, 64, 34, 88, 235, 108, 145, 222, 13, 202, 165, 129, 11, 196, 150,
                    20, 24, 64, 163, 73, 159, 104, 81, 156, 144, 249, 56, 203, 112, 146, 1, 225, 44, 58, 164, 97, 36, 3,
                    199, 57, 160, 69, 156, 78, 0, 216, 0, 95, 143, 80, 234, 13, 0, 2, 140, 100, 168, 11, 126, 64, 65, 0,
                    77, 250, 175, 129, 156, 11, 6, 72, 113, 20, 5, 55, 56, 25, 128, 240, 188, 232, 218, 64, 207, 5, 5,
                    116, 241, 222, 6, 236, 44, 11, 98, 79, 20, 48, 137, 83, 109, 96, 231, 130, 2, 18, 25, 244, 110, 64,
                    10, 2, 104, 200, 175, 21, 167, 149, 129, 157, 11, 10, 224, 182, 184, 1, 47, 232, 0, 94, 245, 194,
                    192, 206, 133, 30, 0, 4, 191, 41, 107, 131, 247, 171, 6, 126, 46, 244, 12, 96, 155, 94, 117, 74,
                    245, 143, 143, 52, 98, 27, 53, 188, 223, 119, 33, 224, 111, 206, 209, 128, 166, 4, 5, 180, 67, 89,
                    142, 152, 59, 187, 11, 12, 177, 180, 159, 242, 195, 192, 105, 26, 12, 96, 221, 105, 79, 211, 190,
                    152, 65, 125, 23, 152, 129, 228, 104, 50, 61, 182, 1, 246, 6, 192, 65, 60, 13, 86, 183, 190, 11, 86,
                    6, 61, 115, 37, 73, 52, 56, 10, 32, 165, 231, 184, 87, 192, 229, 93, 224, 6, 40, 81, 131, 173, 2,
                    246, 6, 64, 159, 162, 190, 214, 159, 239, 2, 55, 224, 9, 220, 123, 51, 24, 12, 48, 99, 82, 41, 182,
                    200, 250, 46, 216, 178, 65, 49, 192, 65, 55, 129, 70, 180, 73, 156, 68, 61, 12, 108, 206, 229, 93,
                    176, 37, 131, 2, 184, 91, 73, 251, 96, 160, 0, 48, 229, 109, 251, 13, 164, 197, 83, 8, 244, 65, 51,
                    232, 210, 86, 228, 109, 150, 120, 165, 222, 103, 252, 240, 119, 196, 196, 187, 246, 68, 79, 106,
                    245, 84, 72, 31, 224, 132, 201, 0, 183, 40, 226, 114, 140, 21, 16, 25, 64, 219, 57, 73, 27, 92, 192,
                    1, 244, 110, 107, 125, 167, 245, 29, 253, 77, 191, 155, 198, 12, 116, 255, 43, 129, 0, 10, 3, 90,
                    92, 218, 90, 223, 73, 189, 3, 110, 64, 96, 3, 235, 159, 11, 56, 32, 78, 41, 225, 87, 167, 128, 174,
                    6, 60, 196, 36, 6, 193, 1, 69, 5, 28, 0, 5, 21, 232, 68, 192, 227, 129, 236, 184, 207, 26, 180, 184,
                    1, 232, 179, 231, 220, 28, 85, 160, 19, 1, 15, 188, 3, 9, 6, 53, 160, 103, 200, 10, 0, 130, 10, 116,
                    42, 224, 128, 105, 138, 202, 238, 18, 247, 244, 1, 153, 58, 218, 99, 200, 27, 201, 253, 249, 34, 12,
                    208, 70, 238, 247, 220, 182, 171, 219, 39, 132, 156, 145, 251, 30, 144, 102, 238, 41, 20, 234, 152,
                    143, 0, 64, 129, 226, 27, 64, 60, 190, 80, 79, 89, 193, 3, 2, 0, 100, 206, 93, 7, 164, 116, 220, 80,
                    79, 161, 48, 207, 186, 58, 226, 40, 128, 13, 114, 215, 13, 154, 38, 2, 128, 158, 10, 193, 2, 245, 0,
                    8, 97, 254, 18, 144, 40, 98, 27, 50, 245, 84, 9, 94, 15, 0, 19, 174, 0, 26, 4, 181, 58, 83, 79, 37,
                    30, 180, 220, 1, 64, 124, 5, 248, 7, 27, 209, 5, 112, 18, 191, 252, 254, 0, 0, 0, 0, 73, 69, 78, 68,
                    174, 66, 96, 130
                }
            },
        };

        private static readonly Dictionary<int, byte[]> EncryptTable = new()
        {
            {
                -2127058559,
                new byte[]
                {
                    42, 249, 232, 90, 127, 226, 133, 63, 140, 196, 207, 234, 229, 148, 119, 45, 128, 17, 141, 135, 0,
                    178, 174, 181, 46, 234, 138, 135, 226, 174, 126, 189, 42, 129, 216, 1, 221, 103, 237, 17, 122, 138,
                    148, 144, 98, 248, 224, 16, 28, 203, 138, 226, 4, 193, 165, 116, 201, 74, 185, 228, 153, 192, 183,
                    154, 188, 9, 66, 222, 164, 103, 38, 110, 231, 60, 40, 25, 172, 153, 115, 208, 168, 79, 25, 16, 107,
                    48, 180, 93, 209, 13, 47, 170, 183, 88, 60, 51, 222, 67, 24, 210, 215, 49, 198, 58, 47, 208, 221,
                    41, 119, 82, 230, 214, 37, 187, 18, 182, 229, 92, 107, 135, 244, 111, 100, 46, 179, 134, 170, 168,
                    141, 198, 114, 161, 5, 81, 49, 191, 229, 134, 154, 129, 95, 191, 59, 119, 254, 126, 7, 165, 199,
                    166, 117, 136, 160, 167, 135, 118, 85, 190, 122, 29, 233, 205, 136, 44, 194, 131, 102, 218, 7, 181,
                    168, 194, 173, 177, 32, 8, 233, 250, 252, 38, 205, 15, 242, 177, 69, 144, 180, 225, 64, 150, 81,
                    112, 135, 193, 142, 47, 196, 3, 81, 60, 217, 216, 195, 84, 20, 230, 24, 59, 146, 184, 121, 191, 136,
                    127, 188, 213, 28, 28, 173, 18, 241, 34, 198, 248, 54, 30, 138, 125, 89, 116, 120, 115, 2, 213, 20,
                    245, 182, 93, 176, 251, 204, 161, 58, 14, 175, 122, 18, 51, 129, 221, 244, 236, 233, 25, 165, 113,
                    187, 93, 226, 68, 147, 251, 4, 47, 98, 102, 173, 110, 174, 63, 214, 160, 221, 202, 253, 213, 131,
                    187, 150, 55, 173, 79, 143, 216, 61, 13, 20, 28, 229, 174, 120, 158, 179, 214, 143, 16, 243, 54,
                    212, 105, 56, 230, 45, 205, 234, 209, 163, 132, 94, 183, 172, 130, 197, 210, 102, 88, 77, 78, 37,
                    141, 255, 196, 160, 156, 43, 210, 222, 70, 58, 184, 131, 108, 233, 35, 177, 216, 140, 214, 35, 99,
                    172, 101, 209, 205, 98, 232, 49, 84, 225, 145, 48, 109, 57, 13, 93, 23, 253, 189, 240, 161, 144, 33,
                    88, 59, 59, 108, 12, 109
                }
            },
            {
                -246049904,
                new byte[]
                {
                    42, 249, 232, 90, 127, 226, 133, 63, 140, 196, 207, 234, 229, 148, 119, 45, 185, 179, 142, 240, 225,
                    190, 91, 249, 120, 122, 38, 24, 51, 215, 211, 137, 55, 111, 79, 181, 187, 211, 79, 198, 40, 73, 58,
                    182, 134, 160, 134, 17, 195, 166, 38, 51, 49, 86, 44, 194, 167, 64, 91, 237, 236, 232, 246, 6, 124,
                    174, 111, 172, 140, 168, 111, 141, 173, 127, 139, 192, 54, 180, 176, 38, 15, 12, 148, 128, 12, 243,
                    195, 103, 236, 15, 77, 147, 82, 106, 168, 38, 92, 182, 211, 248, 146, 95, 133, 66, 107, 214, 245,
                    128, 230, 41, 65, 165, 185, 187, 116, 136, 24, 81, 16, 138, 60, 107, 254, 200, 129, 242, 46, 228,
                    66, 245, 28, 76, 239, 61, 154, 98, 112, 181, 232, 123, 176, 201, 25, 121, 13, 167, 135, 111, 252,
                    30, 196, 189, 219, 178, 43, 45, 220, 102, 245, 105, 191, 78, 8, 209, 192, 133, 237, 28, 195, 36,
                    173, 3, 29, 2, 253, 115, 31, 72, 18, 38, 182, 13, 111, 172, 18, 191, 131, 38, 213, 194, 181, 61, 90,
                    135, 212, 147, 131, 61, 185, 235, 82, 227, 138, 63, 196, 233, 75, 35, 202, 17, 194, 244, 54, 255,
                    90, 102, 151, 126, 139, 189, 213, 131, 137, 93, 19, 23, 247, 230, 141, 119, 33, 160, 42, 44, 107,
                    39, 196, 243, 118, 217, 179, 29, 122, 39, 174, 176, 41, 47, 200, 108, 60, 224, 10, 168, 111, 234,
                    138, 121, 91, 10, 113, 163, 140, 234, 57, 171, 190, 139, 49, 115, 211, 160
                }
            },
        };

        private static readonly Dictionary<int, uint[]> KeyTable = new()
        {
            {
                324082239,
                new uint[]
                {
                    3422807879, 2796534731, 4173231720, 2902376165, 1991065302, 3489826077, 683471733, 2218937744,
                    1481531017, 2286738324, 2700093665, 615671153, 1795423679, 3813615147, 1136594634, 1728932795,
                    3028031290, 1462972689, 344735707, 1937855584, 2824652725, 4285527716, 3957574015, 2556599583,
                    570558451, 3714969943, 915290152, 2934744375, 2885983511, 1902734400, 1206342760, 3909874015,
                    128274953, 1993096777, 824928801, 3626015614, 2880545128, 3715997473, 3965121792, 880176766,
                    2771276400, 2018757969, 2483370065, 2691913263, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                }
            },
            {
                2116979441,
                new uint[]
                {
                    739262252, 1322378652, 189672961, 2182574330, 3712029247, 2475751331, 2564741538, 449423704,
                    43153565, 2432864062, 165435036, 320208836, 1602512096, 3464948702, 3344612674, 3561964166,
                    3551010984, 489606006, 3665065524, 238761138, 565188355, 1017044085, 3874138689, 3906051827,
                    1069226648, 52807405, 3855576236, 220131935, 221435215, 236393378, 3956834062, 3871771985,
                    963577025, 930617187, 3701500013, 979821884, 297201473, 651098146, 4201549903, 3221779827,
                    388718843, 837130457, 3414886550, 193166821, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                }
            },
            {
                -1202486153,
                new uint[]
                {
                    2974162846, 339824211, 413350840, 1277439574, 2255990455, 2453070052, 2325076828, 3333551370,
                    1284622595, 3735497191, 1412625083, 2458060721, 2756968780, 2062756011, 784456208, 3158498721,
                    2157261609, 4200995714, 3567763858, 1759845427, 1225388140, 3010456558, 1741171324, 254621263,
                    2970383386, 40083444, 1705751944, 1787200455, 3051401752, 3078827500, 3525875812, 3098418083,
                    3511662708, 1724683672, 3034844668, 206181983, 470316938, 2059723282, 3458265070, 3261790641,
                    680640431, 1381326269, 2624959059, 1579109346, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                }
            },
        };

        public bool DecryptCbc(byte[] input, int dataSize, out byte[] output, uint[] key, IProgress<int>? progress = null,
            int progressReportInterval = 100, CancellationToken cancellationToken = default)
        {
            var hash = FnvHash.Compute(input.Concat(key.Select(v => (byte)v)).ToArray());
            if (DecryptTable.TryGetValue(hash, out var result))
            {
                output = result;
                return !input.SequenceEqual(output);
            }

            throw new InvalidOperationException("The value is not pre-calculated.");
        }

        public bool EncryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress = null,
            int progressReportInterval = 100, CancellationToken cancellationToken = default)
        {
            var hash = FnvHash.Compute(input.Concat(key.Select(v => (byte)v)).ToArray());
            if (EncryptTable.TryGetValue(hash, out var result))
            {
                output = result;
                return !input.SequenceEqual(output);
            }

            throw new InvalidOperationException("The value is not pre-calculated.");
        }

        public uint[] KeySetup(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Array.Empty<uint>();
            }

            var hash = FnvHash.Compute(Encoding.UTF8.GetBytes(password));
            if (KeyTable.TryGetValue(hash, out var result))
            {
                return result;
            }

            throw new InvalidOperationException("The value is not pre-calculated.");
        }
    }
}