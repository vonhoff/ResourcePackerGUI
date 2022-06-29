﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcePackerGUI.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services
{
    public class Crc32ServiceTests
    {
        private readonly Crc32Service _crc32Service;

        public Crc32ServiceTests()
        {
            _crc32Service = new Crc32Service();
        }

        [Fact]
        public void Compute()
        {
            var result = _crc32Service.Compute(BasePng);
            Assert.Equal((uint)1255023149, result);

            result = _crc32Service.Compute(MushroomRedPng);
            Assert.Equal(4053477474, result);
        }

        #region Raw data

        private static readonly byte[] BasePng = {
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 128,
            0, 0, 0, 16, 4, 3, 0, 0, 0, 44, 118, 106, 187, 0, 0, 0, 48, 80, 76, 84, 69,
            0, 136, 238, 0, 29, 132, 0, 32, 137, 1, 40, 154, 0, 27, 128, 1, 47, 166, 1,
            56, 186, 1, 49, 172, 0, 38, 148, 1, 55, 182, 0, 35, 143, 1, 44, 161, 1, 43,
            158, 1, 53, 178, 1, 52, 176, 0, 34, 141, 61, 118, 69, 151, 0, 0, 0, 249, 73,
            68, 65, 84, 56, 203, 149, 211, 177, 13, 194, 48, 16, 133, 97, 86, 200, 8,
            185, 21, 220, 48, 0, 43, 156, 82, 208, 179, 1, 242, 0, 52, 22, 27, 100, 133,
            244, 84, 89, 193, 43, 120, 5, 23, 94, 0, 222, 225, 7, 138, 18, 132, 225, 75,
            82, 222, 175, 147, 98, 239, 14, 212, 209, 157, 28, 205, 228, 105, 164, 29,
            109, 2, 211, 29, 15, 2, 23, 250, 35, 32, 53, 128, 121, 179, 220, 64, 117,
            214, 118, 64, 170, 137, 28, 41, 220, 110, 218, 14, 72, 95, 173, 3, 71, 176,
            68, 51, 208, 55, 55, 200, 57, 227, 107, 110, 16, 166, 48, 65, 88, 110, 96,
            13, 12, 67, 13, 156, 240, 108, 3, 20, 200, 145, 26, 108, 144, 233, 68, 239,
            64, 247, 250, 7, 210, 139, 185, 210, 34, 0, 95, 2, 7, 20, 186, 15, 27, 68,
            227, 98, 84, 74, 25, 210, 167, 13, 186, 122, 134, 132, 174, 20, 73, 135,
            161, 6, 82, 202, 246, 110, 3, 44, 72, 85, 2, 69, 194, 236, 217, 36, 218, 6,
            72, 74, 41, 82, 68, 214, 1, 44, 224, 189, 122, 109, 6, 48, 43, 40, 172, 54,
            152, 227, 115, 250, 151, 128, 224, 2, 217, 6, 46, 24, 23, 102, 192, 124,
            244, 195, 235, 4, 166, 52, 54, 3, 248, 246, 206, 142, 192, 126, 113, 141,
            135, 167, 17, 5, 36, 198, 117, 224, 1, 119, 108, 147, 212, 68, 91, 165, 163,
            0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
        };

        private static readonly byte[] MushroomRedPng = {
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 23,
            0, 0, 0, 28, 4, 3, 0, 0, 0, 120, 195, 57, 80, 0, 0, 0, 27, 80, 76, 84, 69,
            0, 0, 0, 65, 43, 15, 163, 65, 25, 191, 119, 33, 100, 48, 20, 84, 58, 24,
            108, 72, 20, 148, 109, 39, 119, 61, 23, 105, 144, 33, 67, 0, 0, 0, 1, 116,
            82, 78, 83, 0, 64, 230, 216, 102, 0, 0, 0, 154, 73, 68, 65, 84, 24, 211,
            173, 205, 193, 17, 194, 32, 16, 5, 80, 116, 44, 32, 123, 32, 103, 39, 161,
            2, 215, 131, 71, 81, 10, 144, 195, 218, 129, 164, 2, 39, 237, 251, 191, 144,
            104, 1, 217, 3, 252, 55, 252, 29, 220, 22, 51, 96, 214, 204, 81, 253, 230,
            253, 48, 42, 113, 36, 144, 245, 132, 227, 188, 64, 217, 188, 18, 90, 17, 18,
            86, 8, 198, 20, 137, 17, 43, 41, 138, 136, 115, 55, 214, 19, 98, 238, 42,
            66, 244, 226, 133, 64, 9, 157, 156, 209, 114, 119, 213, 32, 6, 116, 21, 124,
            192, 122, 131, 101, 177, 142, 72, 122, 17, 145, 82, 30, 4, 62, 243, 214,
            151, 249, 69, 28, 248, 95, 63, 205, 111, 2, 106, 104, 179, 243, 19, 106, 43,
            74, 249, 195, 179, 216, 2, 39, 102, 249, 7, 12, 174, 15, 163, 151, 35, 40,
            163, 228, 20, 209, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
        };

        #endregion Raw data
    }
}
