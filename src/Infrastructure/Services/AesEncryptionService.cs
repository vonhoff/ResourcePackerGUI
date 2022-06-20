﻿#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Utilities;

namespace ResourcePackerGUI.Infrastructure.Services
{
    internal class AesEncryptionService : IAesEncryptionService
    {
        /// <summary>
        /// AES operates on 16 bytes at a time.
        /// </summary>
        public const int BlockSize = 16;

        /// <summary>
        /// This table stores pre-calculated values for all possible GF(2^8) calculations.This
        /// table is only used by the (Inv)MixColumns steps.
        /// </summary>
        private static readonly byte[][] GfMul =
        {
            new byte[]{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, new byte[]{ 0x02, 0x03, 0x09, 0x0b, 0x0d, 0x0e },
            new byte[]{ 0x04, 0x06, 0x12, 0x16, 0x1a, 0x1c }, new byte[]{ 0x06, 0x05, 0x1b, 0x1d, 0x17, 0x12 },
            new byte[]{ 0x08, 0x0c, 0x24, 0x2c, 0x34, 0x38 }, new byte[]{ 0x0a, 0x0f, 0x2d, 0x27, 0x39, 0x36 },
            new byte[]{ 0x0c, 0x0a, 0x36, 0x3a, 0x2e, 0x24 }, new byte[]{ 0x0e, 0x09, 0x3f, 0x31, 0x23, 0x2a },
            new byte[]{ 0x10, 0x18, 0x48, 0x58, 0x68, 0x70 }, new byte[]{ 0x12, 0x1b, 0x41, 0x53, 0x65, 0x7e },
            new byte[]{ 0x14, 0x1e, 0x5a, 0x4e, 0x72, 0x6c }, new byte[]{ 0x16, 0x1d, 0x53, 0x45, 0x7f, 0x62 },
            new byte[]{ 0x18, 0x14, 0x6c, 0x74, 0x5c, 0x48 }, new byte[]{ 0x1a, 0x17, 0x65, 0x7f, 0x51, 0x46 },
            new byte[]{ 0x1c, 0x12, 0x7e, 0x62, 0x46, 0x54 }, new byte[]{ 0x1e, 0x11, 0x77, 0x69, 0x4b, 0x5a },
            new byte[]{ 0x20, 0x30, 0x90, 0xb0, 0xd0, 0xe0 }, new byte[]{ 0x22, 0x33, 0x99, 0xbb, 0xdd, 0xee },
            new byte[]{ 0x24, 0x36, 0x82, 0xa6, 0xca, 0xfc }, new byte[]{ 0x26, 0x35, 0x8b, 0xad, 0xc7, 0xf2 },
            new byte[]{ 0x28, 0x3c, 0xb4, 0x9c, 0xe4, 0xd8 }, new byte[]{ 0x2a, 0x3f, 0xbd, 0x97, 0xe9, 0xd6 },
            new byte[]{ 0x2c, 0x3a, 0xa6, 0x8a, 0xfe, 0xc4 }, new byte[]{ 0x2e, 0x39, 0xaf, 0x81, 0xf3, 0xca },
            new byte[]{ 0x30, 0x28, 0xd8, 0xe8, 0xb8, 0x90 }, new byte[]{ 0x32, 0x2b, 0xd1, 0xe3, 0xb5, 0x9e },
            new byte[]{ 0x34, 0x2e, 0xca, 0xfe, 0xa2, 0x8c }, new byte[]{ 0x36, 0x2d, 0xc3, 0xf5, 0xaf, 0x82 },
            new byte[]{ 0x38, 0x24, 0xfc, 0xc4, 0x8c, 0xa8 }, new byte[]{ 0x3a, 0x27, 0xf5, 0xcf, 0x81, 0xa6 },
            new byte[]{ 0x3c, 0x22, 0xee, 0xd2, 0x96, 0xb4 }, new byte[]{ 0x3e, 0x21, 0xe7, 0xd9, 0x9b, 0xba },
            new byte[]{ 0x40, 0x60, 0x3b, 0x7b, 0xbb, 0xdb }, new byte[]{ 0x42, 0x63, 0x32, 0x70, 0xb6, 0xd5 },
            new byte[]{ 0x44, 0x66, 0x29, 0x6d, 0xa1, 0xc7 }, new byte[]{ 0x46, 0x65, 0x20, 0x66, 0xac, 0xc9 },
            new byte[]{ 0x48, 0x6c, 0x1f, 0x57, 0x8f, 0xe3 }, new byte[]{ 0x4a, 0x6f, 0x16, 0x5c, 0x82, 0xed },
            new byte[]{ 0x4c, 0x6a, 0x0d, 0x41, 0x95, 0xff }, new byte[]{ 0x4e, 0x69, 0x04, 0x4a, 0x98, 0xf1 },
            new byte[]{ 0x50, 0x78, 0x73, 0x23, 0xd3, 0xab }, new byte[]{ 0x52, 0x7b, 0x7a, 0x28, 0xde, 0xa5 },
            new byte[]{ 0x54, 0x7e, 0x61, 0x35, 0xc9, 0xb7 }, new byte[]{ 0x56, 0x7d, 0x68, 0x3e, 0xc4, 0xb9 },
            new byte[]{ 0x58, 0x74, 0x57, 0x0f, 0xe7, 0x93 }, new byte[]{ 0x5a, 0x77, 0x5e, 0x04, 0xea, 0x9d },
            new byte[]{ 0x5c, 0x72, 0x45, 0x19, 0xfd, 0x8f }, new byte[]{ 0x5e, 0x71, 0x4c, 0x12, 0xf0, 0x81 },
            new byte[]{ 0x60, 0x50, 0xab, 0xcb, 0x6b, 0x3b }, new byte[]{ 0x62, 0x53, 0xa2, 0xc0, 0x66, 0x35 },
            new byte[]{ 0x64, 0x56, 0xb9, 0xdd, 0x71, 0x27 }, new byte[]{ 0x66, 0x55, 0xb0, 0xd6, 0x7c, 0x29 },
            new byte[]{ 0x68, 0x5c, 0x8f, 0xe7, 0x5f, 0x03 }, new byte[]{ 0x6a, 0x5f, 0x86, 0xec, 0x52, 0x0d },
            new byte[]{ 0x6c, 0x5a, 0x9d, 0xf1, 0x45, 0x1f }, new byte[]{ 0x6e, 0x59, 0x94, 0xfa, 0x48, 0x11 },
            new byte[]{ 0x70, 0x48, 0xe3, 0x93, 0x03, 0x4b }, new byte[]{ 0x72, 0x4b, 0xea, 0x98, 0x0e, 0x45 },
            new byte[]{ 0x74, 0x4e, 0xf1, 0x85, 0x19, 0x57 }, new byte[]{ 0x76, 0x4d, 0xf8, 0x8e, 0x14, 0x59 },
            new byte[]{ 0x78, 0x44, 0xc7, 0xbf, 0x37, 0x73 }, new byte[]{ 0x7a, 0x47, 0xce, 0xb4, 0x3a, 0x7d },
            new byte[]{ 0x7c, 0x42, 0xd5, 0xa9, 0x2d, 0x6f }, new byte[]{ 0x7e, 0x41, 0xdc, 0xa2, 0x20, 0x61 },
            new byte[]{ 0x80, 0xc0, 0x76, 0xf6, 0x6d, 0xad }, new byte[]{ 0x82, 0xc3, 0x7f, 0xfd, 0x60, 0xa3 },
            new byte[]{ 0x84, 0xc6, 0x64, 0xe0, 0x77, 0xb1 }, new byte[]{ 0x86, 0xc5, 0x6d, 0xeb, 0x7a, 0xbf },
            new byte[]{ 0x88, 0xcc, 0x52, 0xda, 0x59, 0x95 }, new byte[]{ 0x8a, 0xcf, 0x5b, 0xd1, 0x54, 0x9b },
            new byte[]{ 0x8c, 0xca, 0x40, 0xcc, 0x43, 0x89 }, new byte[]{ 0x8e, 0xc9, 0x49, 0xc7, 0x4e, 0x87 },
            new byte[]{ 0x90, 0xd8, 0x3e, 0xae, 0x05, 0xdd }, new byte[]{ 0x92, 0xdb, 0x37, 0xa5, 0x08, 0xd3 },
            new byte[]{ 0x94, 0xde, 0x2c, 0xb8, 0x1f, 0xc1 }, new byte[]{ 0x96, 0xdd, 0x25, 0xb3, 0x12, 0xcf },
            new byte[]{ 0x98, 0xd4, 0x1a, 0x82, 0x31, 0xe5 }, new byte[]{ 0x9a, 0xd7, 0x13, 0x89, 0x3c, 0xeb },
            new byte[]{ 0x9c, 0xd2, 0x08, 0x94, 0x2b, 0xf9 }, new byte[]{ 0x9e, 0xd1, 0x01, 0x9f, 0x26, 0xf7 },
            new byte[]{ 0xa0, 0xf0, 0xe6, 0x46, 0xbd, 0x4d }, new byte[]{ 0xa2, 0xf3, 0xef, 0x4d, 0xb0, 0x43 },
            new byte[]{ 0xa4, 0xf6, 0xf4, 0x50, 0xa7, 0x51 }, new byte[]{ 0xa6, 0xf5, 0xfd, 0x5b, 0xaa, 0x5f },
            new byte[]{ 0xa8, 0xfc, 0xc2, 0x6a, 0x89, 0x75 }, new byte[]{ 0xaa, 0xff, 0xcb, 0x61, 0x84, 0x7b },
            new byte[]{ 0xac, 0xfa, 0xd0, 0x7c, 0x93, 0x69 }, new byte[]{ 0xae, 0xf9, 0xd9, 0x77, 0x9e, 0x67 },
            new byte[]{ 0xb0, 0xe8, 0xae, 0x1e, 0xd5, 0x3d }, new byte[]{ 0xb2, 0xeb, 0xa7, 0x15, 0xd8, 0x33 },
            new byte[]{ 0xb4, 0xee, 0xbc, 0x08, 0xcf, 0x21 }, new byte[]{ 0xb6, 0xed, 0xb5, 0x03, 0xc2, 0x2f },
            new byte[]{ 0xb8, 0xe4, 0x8a, 0x32, 0xe1, 0x05 }, new byte[]{ 0xba, 0xe7, 0x83, 0x39, 0xec, 0x0b },
            new byte[]{ 0xbc, 0xe2, 0x98, 0x24, 0xfb, 0x19 }, new byte[]{ 0xbe, 0xe1, 0x91, 0x2f, 0xf6, 0x17 },
            new byte[]{ 0xc0, 0xa0, 0x4d, 0x8d, 0xd6, 0x76 }, new byte[]{ 0xc2, 0xa3, 0x44, 0x86, 0xdb, 0x78 },
            new byte[]{ 0xc4, 0xa6, 0x5f, 0x9b, 0xcc, 0x6a }, new byte[]{ 0xc6, 0xa5, 0x56, 0x90, 0xc1, 0x64 },
            new byte[]{ 0xc8, 0xac, 0x69, 0xa1, 0xe2, 0x4e }, new byte[]{ 0xca, 0xaf, 0x60, 0xaa, 0xef, 0x40 },
            new byte[]{ 0xcc, 0xaa, 0x7b, 0xb7, 0xf8, 0x52 }, new byte[]{ 0xce, 0xa9, 0x72, 0xbc, 0xf5, 0x5c },
            new byte[]{ 0xd0, 0xb8, 0x05, 0xd5, 0xbe, 0x06 }, new byte[]{ 0xd2, 0xbb, 0x0c, 0xde, 0xb3, 0x08 },
            new byte[]{ 0xd4, 0xbe, 0x17, 0xc3, 0xa4, 0x1a }, new byte[]{ 0xd6, 0xbd, 0x1e, 0xc8, 0xa9, 0x14 },
            new byte[]{ 0xd8, 0xb4, 0x21, 0xf9, 0x8a, 0x3e }, new byte[]{ 0xda, 0xb7, 0x28, 0xf2, 0x87, 0x30 },
            new byte[]{ 0xdc, 0xb2, 0x33, 0xef, 0x90, 0x22 }, new byte[]{ 0xde, 0xb1, 0x3a, 0xe4, 0x9d, 0x2c },
            new byte[]{ 0xe0, 0x90, 0xdd, 0x3d, 0x06, 0x96 }, new byte[]{ 0xe2, 0x93, 0xd4, 0x36, 0x0b, 0x98 },
            new byte[]{ 0xe4, 0x96, 0xcf, 0x2b, 0x1c, 0x8a }, new byte[]{ 0xe6, 0x95, 0xc6, 0x20, 0x11, 0x84 },
            new byte[]{ 0xe8, 0x9c, 0xf9, 0x11, 0x32, 0xae }, new byte[]{ 0xea, 0x9f, 0xf0, 0x1a, 0x3f, 0xa0 },
            new byte[]{ 0xec, 0x9a, 0xeb, 0x07, 0x28, 0xb2 }, new byte[]{ 0xee, 0x99, 0xe2, 0x0c, 0x25, 0xbc },
            new byte[]{ 0xf0, 0x88, 0x95, 0x65, 0x6e, 0xe6 }, new byte[]{ 0xf2, 0x8b, 0x9c, 0x6e, 0x63, 0xe8 },
            new byte[]{ 0xf4, 0x8e, 0x87, 0x73, 0x74, 0xfa }, new byte[]{ 0xf6, 0x8d, 0x8e, 0x78, 0x79, 0xf4 },
            new byte[]{ 0xf8, 0x84, 0xb1, 0x49, 0x5a, 0xde }, new byte[]{ 0xfa, 0x87, 0xb8, 0x42, 0x57, 0xd0 },
            new byte[]{ 0xfc, 0x82, 0xa3, 0x5f, 0x40, 0xc2 }, new byte[]{ 0xfe, 0x81, 0xaa, 0x54, 0x4d, 0xcc },
            new byte[]{ 0x1b, 0x9b, 0xec, 0xf7, 0xda, 0x41 }, new byte[]{ 0x19, 0x98, 0xe5, 0xfc, 0xd7, 0x4f },
            new byte[]{ 0x1f, 0x9d, 0xfe, 0xe1, 0xc0, 0x5d }, new byte[]{ 0x1d, 0x9e, 0xf7, 0xea, 0xcd, 0x53 },
            new byte[]{ 0x13, 0x97, 0xc8, 0xdb, 0xee, 0x79 }, new byte[]{ 0x11, 0x94, 0xc1, 0xd0, 0xe3, 0x77 },
            new byte[]{ 0x17, 0x91, 0xda, 0xcd, 0xf4, 0x65 }, new byte[]{ 0x15, 0x92, 0xd3, 0xc6, 0xf9, 0x6b },
            new byte[]{ 0x0b, 0x83, 0xa4, 0xaf, 0xb2, 0x31 }, new byte[]{ 0x09, 0x80, 0xad, 0xa4, 0xbf, 0x3f },
            new byte[]{ 0x0f, 0x85, 0xb6, 0xb9, 0xa8, 0x2d }, new byte[]{ 0x0d, 0x86, 0xbf, 0xb2, 0xa5, 0x23 },
            new byte[]{ 0x03, 0x8f, 0x80, 0x83, 0x86, 0x09 }, new byte[]{ 0x01, 0x8c, 0x89, 0x88, 0x8b, 0x07 },
            new byte[]{ 0x07, 0x89, 0x92, 0x95, 0x9c, 0x15 }, new byte[]{ 0x05, 0x8a, 0x9b, 0x9e, 0x91, 0x1b },
            new byte[]{ 0x3b, 0xab, 0x7c, 0x47, 0x0a, 0xa1 }, new byte[]{ 0x39, 0xa8, 0x75, 0x4c, 0x07, 0xaf },
            new byte[]{ 0x3f, 0xad, 0x6e, 0x51, 0x10, 0xbd }, new byte[]{ 0x3d, 0xae, 0x67, 0x5a, 0x1d, 0xb3 },
            new byte[]{ 0x33, 0xa7, 0x58, 0x6b, 0x3e, 0x99 }, new byte[]{ 0x31, 0xa4, 0x51, 0x60, 0x33, 0x97 },
            new byte[]{ 0x37, 0xa1, 0x4a, 0x7d, 0x24, 0x85 }, new byte[]{ 0x35, 0xa2, 0x43, 0x76, 0x29, 0x8b },
            new byte[]{ 0x2b, 0xb3, 0x34, 0x1f, 0x62, 0xd1 }, new byte[]{ 0x29, 0xb0, 0x3d, 0x14, 0x6f, 0xdf },
            new byte[]{ 0x2f, 0xb5, 0x26, 0x09, 0x78, 0xcd }, new byte[]{ 0x2d, 0xb6, 0x2f, 0x02, 0x75, 0xc3 },
            new byte[]{ 0x23, 0xbf, 0x10, 0x33, 0x56, 0xe9 }, new byte[]{ 0x21, 0xbc, 0x19, 0x38, 0x5b, 0xe7 },
            new byte[]{ 0x27, 0xb9, 0x02, 0x25, 0x4c, 0xf5 }, new byte[]{ 0x25, 0xba, 0x0b, 0x2e, 0x41, 0xfb },
            new byte[]{ 0x5b, 0xfb, 0xd7, 0x8c, 0x61, 0x9a }, new byte[]{ 0x59, 0xf8, 0xde, 0x87, 0x6c, 0x94 },
            new byte[]{ 0x5f, 0xfd, 0xc5, 0x9a, 0x7b, 0x86 }, new byte[]{ 0x5d, 0xfe, 0xcc, 0x91, 0x76, 0x88 },
            new byte[]{ 0x53, 0xf7, 0xf3, 0xa0, 0x55, 0xa2 }, new byte[]{ 0x51, 0xf4, 0xfa, 0xab, 0x58, 0xac },
            new byte[]{ 0x57, 0xf1, 0xe1, 0xb6, 0x4f, 0xbe }, new byte[]{ 0x55, 0xf2, 0xe8, 0xbd, 0x42, 0xb0 },
            new byte[]{ 0x4b, 0xe3, 0x9f, 0xd4, 0x09, 0xea }, new byte[]{ 0x49, 0xe0, 0x96, 0xdf, 0x04, 0xe4 },
            new byte[]{ 0x4f, 0xe5, 0x8d, 0xc2, 0x13, 0xf6 }, new byte[]{ 0x4d, 0xe6, 0x84, 0xc9, 0x1e, 0xf8 },
            new byte[]{ 0x43, 0xef, 0xbb, 0xf8, 0x3d, 0xd2 }, new byte[]{ 0x41, 0xec, 0xb2, 0xf3, 0x30, 0xdc },
            new byte[]{ 0x47, 0xe9, 0xa9, 0xee, 0x27, 0xce }, new byte[]{ 0x45, 0xea, 0xa0, 0xe5, 0x2a, 0xc0 },
            new byte[]{ 0x7b, 0xcb, 0x47, 0x3c, 0xb1, 0x7a }, new byte[]{ 0x79, 0xc8, 0x4e, 0x37, 0xbc, 0x74 },
            new byte[]{ 0x7f, 0xcd, 0x55, 0x2a, 0xab, 0x66 }, new byte[]{ 0x7d, 0xce, 0x5c, 0x21, 0xa6, 0x68 },
            new byte[]{ 0x73, 0xc7, 0x63, 0x10, 0x85, 0x42 }, new byte[]{ 0x71, 0xc4, 0x6a, 0x1b, 0x88, 0x4c },
            new byte[]{ 0x77, 0xc1, 0x71, 0x06, 0x9f, 0x5e }, new byte[]{ 0x75, 0xc2, 0x78, 0x0d, 0x92, 0x50 },
            new byte[]{ 0x6b, 0xd3, 0x0f, 0x64, 0xd9, 0x0a }, new byte[]{ 0x69, 0xd0, 0x06, 0x6f, 0xd4, 0x04 },
            new byte[]{ 0x6f, 0xd5, 0x1d, 0x72, 0xc3, 0x16 }, new byte[]{ 0x6d, 0xd6, 0x14, 0x79, 0xce, 0x18 },
            new byte[]{ 0x63, 0xdf, 0x2b, 0x48, 0xed, 0x32 }, new byte[]{ 0x61, 0xdc, 0x22, 0x43, 0xe0, 0x3c },
            new byte[]{ 0x67, 0xd9, 0x39, 0x5e, 0xf7, 0x2e }, new byte[]{ 0x65, 0xda, 0x30, 0x55, 0xfa, 0x20 },
            new byte[]{ 0x9b, 0x5b, 0x9a, 0x01, 0xb7, 0xec }, new byte[]{ 0x99, 0x58, 0x93, 0x0a, 0xba, 0xe2 },
            new byte[]{ 0x9f, 0x5d, 0x88, 0x17, 0xad, 0xf0 }, new byte[]{ 0x9d, 0x5e, 0x81, 0x1c, 0xa0, 0xfe },
            new byte[]{ 0x93, 0x57, 0xbe, 0x2d, 0x83, 0xd4 }, new byte[]{ 0x91, 0x54, 0xb7, 0x26, 0x8e, 0xda },
            new byte[]{ 0x97, 0x51, 0xac, 0x3b, 0x99, 0xc8 }, new byte[]{ 0x95, 0x52, 0xa5, 0x30, 0x94, 0xc6 },
            new byte[]{ 0x8b, 0x43, 0xd2, 0x59, 0xdf, 0x9c }, new byte[]{ 0x89, 0x40, 0xdb, 0x52, 0xd2, 0x92 },
            new byte[]{ 0x8f, 0x45, 0xc0, 0x4f, 0xc5, 0x80 }, new byte[]{ 0x8d, 0x46, 0xc9, 0x44, 0xc8, 0x8e },
            new byte[]{ 0x83, 0x4f, 0xf6, 0x75, 0xeb, 0xa4 }, new byte[]{ 0x81, 0x4c, 0xff, 0x7e, 0xe6, 0xaa },
            new byte[]{ 0x87, 0x49, 0xe4, 0x63, 0xf1, 0xb8 }, new byte[]{ 0x85, 0x4a, 0xed, 0x68, 0xfc, 0xb6 },
            new byte[]{ 0xbb, 0x6b, 0x0a, 0xb1, 0x67, 0x0c }, new byte[]{ 0xb9, 0x68, 0x03, 0xba, 0x6a, 0x02 },
            new byte[]{ 0xbf, 0x6d, 0x18, 0xa7, 0x7d, 0x10 }, new byte[]{ 0xbd, 0x6e, 0x11, 0xac, 0x70, 0x1e },
            new byte[]{ 0xb3, 0x67, 0x2e, 0x9d, 0x53, 0x34 }, new byte[]{ 0xb1, 0x64, 0x27, 0x96, 0x5e, 0x3a },
            new byte[]{ 0xb7, 0x61, 0x3c, 0x8b, 0x49, 0x28 }, new byte[]{ 0xb5, 0x62, 0x35, 0x80, 0x44, 0x26 },
            new byte[]{ 0xab, 0x73, 0x42, 0xe9, 0x0f, 0x7c }, new byte[]{ 0xa9, 0x70, 0x4b, 0xe2, 0x02, 0x72 },
            new byte[]{ 0xaf, 0x75, 0x50, 0xff, 0x15, 0x60 }, new byte[]{ 0xad, 0x76, 0x59, 0xf4, 0x18, 0x6e },
            new byte[]{ 0xa3, 0x7f, 0x66, 0xc5, 0x3b, 0x44 }, new byte[]{ 0xa1, 0x7c, 0x6f, 0xce, 0x36, 0x4a },
            new byte[]{ 0xa7, 0x79, 0x74, 0xd3, 0x21, 0x58 }, new byte[]{ 0xa5, 0x7a, 0x7d, 0xd8, 0x2c, 0x56 },
            new byte[]{ 0xdb, 0x3b, 0xa1, 0x7a, 0x0c, 0x37 }, new byte[]{ 0xd9, 0x38, 0xa8, 0x71, 0x01, 0x39 },
            new byte[]{ 0xdf, 0x3d, 0xb3, 0x6c, 0x16, 0x2b }, new byte[]{ 0xdd, 0x3e, 0xba, 0x67, 0x1b, 0x25 },
            new byte[]{ 0xd3, 0x37, 0x85, 0x56, 0x38, 0x0f }, new byte[]{ 0xd1, 0x34, 0x8c, 0x5d, 0x35, 0x01 },
            new byte[]{ 0xd7, 0x31, 0x97, 0x40, 0x22, 0x13 }, new byte[]{ 0xd5, 0x32, 0x9e, 0x4b, 0x2f, 0x1d },
            new byte[]{ 0xcb, 0x23, 0xe9, 0x22, 0x64, 0x47 }, new byte[]{ 0xc9, 0x20, 0xe0, 0x29, 0x69, 0x49 },
            new byte[]{ 0xcf, 0x25, 0xfb, 0x34, 0x7e, 0x5b }, new byte[]{ 0xcd, 0x26, 0xf2, 0x3f, 0x73, 0x55 },
            new byte[]{ 0xc3, 0x2f, 0xcd, 0x0e, 0x50, 0x7f }, new byte[]{ 0xc1, 0x2c, 0xc4, 0x05, 0x5d, 0x71 },
            new byte[]{ 0xc7, 0x29, 0xdf, 0x18, 0x4a, 0x63 }, new byte[]{ 0xc5, 0x2a, 0xd6, 0x13, 0x47, 0x6d },
            new byte[]{ 0xfb, 0x0b, 0x31, 0xca, 0xdc, 0xd7 }, new byte[]{ 0xf9, 0x08, 0x38, 0xc1, 0xd1, 0xd9 },
            new byte[]{ 0xff, 0x0d, 0x23, 0xdc, 0xc6, 0xcb }, new byte[]{ 0xfd, 0x0e, 0x2a, 0xd7, 0xcb, 0xc5 },
            new byte[]{ 0xf3, 0x07, 0x15, 0xe6, 0xe8, 0xef }, new byte[]{ 0xf1, 0x04, 0x1c, 0xed, 0xe5, 0xe1 },
            new byte[]{ 0xf7, 0x01, 0x07, 0xf0, 0xf2, 0xf3 }, new byte[]{ 0xf5, 0x02, 0x0e, 0xfb, 0xff, 0xfd },
            new byte[]{ 0xeb, 0x13, 0x79, 0x92, 0xb4, 0xa7 }, new byte[]{ 0xe9, 0x10, 0x70, 0x99, 0xb9, 0xa9 },
            new byte[]{ 0xef, 0x15, 0x6b, 0x84, 0xae, 0xbb }, new byte[]{ 0xed, 0x16, 0x62, 0x8f, 0xa3, 0xb5 },
            new byte[]{ 0xe3, 0x1f, 0x5d, 0xbe, 0x80, 0x9f }, new byte[]{ 0xe1, 0x1c, 0x54, 0xb5, 0x8d, 0x91 },
            new byte[]{ 0xe7, 0x19, 0x4f, 0xa8, 0x9a, 0x83 }, new byte[]{ 0xe5, 0x1a, 0x46, 0xa3, 0x97, 0x8d }
        };

        private static readonly byte[][] InvSBox = {
            new byte[]{ 0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB},
            new byte[]{ 0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB},
            new byte[]{ 0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E},
            new byte[]{ 0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25},
            new byte[]{ 0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92},
            new byte[]{ 0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84},
            new byte[]{ 0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06},
            new byte[]{ 0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B},
            new byte[]{ 0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73},
            new byte[]{ 0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E},
            new byte[]{ 0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B},
            new byte[]{ 0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4},
            new byte[]{ 0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F},
            new byte[]{ 0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF},
            new byte[]{ 0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61},
            new byte[]{ 0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D}
        };

        /// <summary>
        /// Default IV when a custom value is not assigned.
        /// </summary>
        private static readonly byte[] Iv = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

        /// <summary>
        /// This is the specified AES SBox. To look up a substitution value, put the first
        /// nibble in the first index (row) and the second nibble in the second index (column).
        /// </summary>
        private static readonly byte[][] SBox = {
            new byte[]{ 0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76 },
            new byte[]{ 0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0},
            new byte[]{ 0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15},
            new byte[]{ 0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75},
            new byte[]{ 0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84},
            new byte[]{ 0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF},
            new byte[]{ 0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8},
            new byte[]{ 0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2},
            new byte[]{ 0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73},
            new byte[]{ 0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB},
            new byte[]{ 0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79},
            new byte[]{ 0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08},
            new byte[]{ 0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A},
            new byte[]{ 0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E},
            new byte[]{ 0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF},
            new byte[]{ 0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16}
        };

        public bool DecryptCbc(byte[] input, out byte[] output, uint[] key,
                    IProgress<int>? progress = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default)
        {
            var packSize = input.Length;
            if (packSize % BlockSize != 0)
            {
                output = Array.Empty<byte>();
                return false;
            }

            output = new byte[packSize];
            var inputBuffer = new byte[BlockSize];
            var outputBuffer = new byte[BlockSize];
            var ivBuffer = new byte[BlockSize];
            var percentage = 0;

            using var progressTimer = new System.Timers.Timer(progressReportInterval);
            progressTimer.Elapsed += delegate
            {
                // ReSharper disable once AccessToModifiedClosure
                progress!.Report(percentage);
            };
            progressTimer.Enabled = progress != null;

            MemoryUtility.CopyMemory(Iv, 0, ivBuffer, 0, BlockSize);

            var blocks = packSize / BlockSize;
            for (var i = 0; i < blocks; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                MemoryUtility.CopyMemory(input, i * BlockSize, inputBuffer, 0, BlockSize);
                Decrypt(inputBuffer, ref outputBuffer, key);
                XorBuf(ivBuffer, ref outputBuffer, BlockSize);
                MemoryUtility.CopyMemory(outputBuffer, 0, output, i * BlockSize, BlockSize);
                MemoryUtility.CopyMemory(inputBuffer, 0, ivBuffer, 0, BlockSize);
                percentage = (int)((double)(i + 1) / blocks * 100);
            }

            progress?.Report(100);
            return true;
        }

        public bool EncryptCbc(byte[] input, out byte[] output, uint[] key,
            IProgress<int>? progress = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default)
        {
            var packSize = (input.Length + BlockSize - 1) & ~(BlockSize - 1);
            if (packSize == input.Length)
            {
                packSize += BlockSize;
            }

            if (packSize % BlockSize != 0)
            {
                output = Array.Empty<byte>();
                return false;
            }

            input = ApplyPadding(input, packSize);
            output = new byte[packSize];
            var inputBuffer = new byte[BlockSize];
            var outputBuffer = new byte[BlockSize];
            var ivBuffer = new byte[BlockSize];
            var percentage = 0;

            using var progressUpdateTimer = new System.Timers.Timer(progressReportInterval);
            progressUpdateTimer.Elapsed += delegate
            {
                // ReSharper disable once AccessToModifiedClosure
                progress!.Report(percentage);
            };
            progressUpdateTimer.Enabled = progress != null;

            MemoryUtility.CopyMemory(Iv, 0, ivBuffer, 0, BlockSize);

            var blocks = packSize / BlockSize;
            for (var i = 0; i < blocks; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                MemoryUtility.CopyMemory(input, i * BlockSize, inputBuffer, 0, BlockSize);
                XorBuf(ivBuffer, ref inputBuffer, BlockSize);
                Encrypt(inputBuffer, ref outputBuffer, key);
                MemoryUtility.CopyMemory(outputBuffer, 0, output, i * BlockSize, BlockSize);
                MemoryUtility.CopyMemory(outputBuffer, 0, ivBuffer, 0, BlockSize);
                percentage = (int)((double)(i + 1) / blocks * 100);
            }

            progress?.Report(100);
            return true;
        }

        public uint[] KeySetup(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Array.Empty<uint>();
            }

            byte[] key;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                key = md5.ComputeHash(inputBytes);
            }

            uint[] rcon = { 0x01000000,0x02000000,0x04000000,0x08000000,0x10000000,0x20000000,
                0x40000000,0x80000000,0x1b000000,0x36000000,0x6c000000,0xd8000000,
                0xab000000,0x4d000000,0x9a000000 };

            var result = new uint[60];
            for (var index = 0; index < 4; ++index)
            {
                result[index] = (uint)((key[4 * index] << 24) | (key[(4 * index) + 1] << 16) |
                                  (key[(4 * index) + 2] << 8) | key[(4 * index) + 3]);
            }

            for (var index = 4; index < 44; ++index)
            {
                var temp = result[index - 1];
                if (index % 4 == 0)
                {
                    temp = SubWord(RotateWord(temp)) ^ rcon[(index - 1) / 4];
                }

                result[index] = result[index - 4] ^ temp;
            }

            return result;
        }

        private static void AddRoundKey(ref byte[][] state, IReadOnlyList<uint> w)
        {
            var subKey = new byte[4];

            for (var i = 0; i < 4; ++i)
            {
                subKey[0] = (byte)(w[i] >> 24);
                subKey[1] = (byte)(w[i] >> 16);
                subKey[2] = (byte)(w[i] >> 8);
                subKey[3] = (byte)w[i];
                state[0][i] ^= subKey[0];
                state[1][i] ^= subKey[1];
                state[2][i] ^= subKey[2];
                state[3][i] ^= subKey[3];
            }
        }

        private static byte[] ApplyPadding(IReadOnlyCollection<byte> data, int packSize)
        {
            var padding = packSize - data.Count;
            var pkcs7 = Enumerable.Repeat((byte)padding, padding);
            return data.Concat(pkcs7).ToArray();
        }

        private static byte[][] CopyInputToState(IReadOnlyList<byte> input)
        {
            var state = new byte[4][];
            var k = 0;
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    if (i == 0)
                    {
                        state[j] = new byte[4];
                    }

                    state[j][i] = input[k++];
                }
            }

            return state;
        }

        private static void CopyStateToOutput(ref byte[] output, IReadOnlyList<byte[]> state)
        {
            var k = 0;
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    output[k++] = state[j][i];
                }
            }
        }

        private static void Decrypt(IReadOnlyList<byte> input, ref byte[] output, uint[] key)
        {
            // Copy input array (should be 16 bytes long) to a matrix
            // (sequential bytes are ordered by row, not col) called "state" for processing.
            var state = CopyInputToState(input);

            // Perform the necessary number of rounds. The round key is added first.
            // The last round does not perform the MixColumns step.
            for (var i = 40; i >= 0; i -= 4)
            {
                if (i < 40)
                {
                    InvShiftRows(ref state);
                    InvSubBytes(ref state);
                }

                AddRoundKey(ref state, key[i..]);

                if (i is < 40 and > 0)
                {
                    InvMixColumns(ref state);
                }
            }

            // Copy the state to the output array.
            CopyStateToOutput(ref output, state);
        }

        private static void Encrypt(IReadOnlyList<byte> input, ref byte[] output, uint[] key)
        {
            // Copy input array (should be 16 bytes long) to a matrix
            // (sequential bytes are ordered by row, not col) called "state" for processing.
            var state = CopyInputToState(input);

            // Perform the necessary number of rounds. The round key is added first.
            // The last round does not perform the MixColumns step.
            for (var i = 0; i <= 40; i += 4)
            {
                if (i > 0)
                {
                    SubBytes(ref state);
                    ShiftRows(ref state);
                }

                if (i is > 0 and < 40)
                {
                    MixColumns(ref state);
                }

                AddRoundKey(ref state, key[i..]);
            }

            // Copy the state to the output array.
            CopyStateToOutput(ref output, state);
        }

        private static void InvMixColumns(ref byte[][] state)
        {
            var col = new byte[4];
            for (var i = 0; i < 4; ++i)
            {
                col[0] = state[0][i];
                col[1] = state[1][i];
                col[2] = state[2][i];
                col[3] = state[3][i];
                state[0][i] = GfMul[col[0]][5];
                state[0][i] ^= GfMul[col[1]][3];
                state[0][i] ^= GfMul[col[2]][4];
                state[0][i] ^= GfMul[col[3]][2];
                state[1][i] = GfMul[col[0]][2];
                state[1][i] ^= GfMul[col[1]][5];
                state[1][i] ^= GfMul[col[2]][3];
                state[1][i] ^= GfMul[col[3]][4];
                state[2][i] = GfMul[col[0]][4];
                state[2][i] ^= GfMul[col[1]][2];
                state[2][i] ^= GfMul[col[2]][5];
                state[2][i] ^= GfMul[col[3]][3];
                state[3][i] = GfMul[col[0]][3];
                state[3][i] ^= GfMul[col[1]][4];
                state[3][i] ^= GfMul[col[2]][2];
                state[3][i] ^= GfMul[col[3]][5];
            }
        }

        /// <summary>
        /// All rows are shifted cylindrically to the right.
        /// </summary>
        /// <param name="state">The state for which to perform the inverted shift-rows step.</param>
        private static void InvShiftRows(ref byte[][] state)
        {
            // Shift right by 1
            var t = state[1][3];
            state[1][3] = state[1][2];
            state[1][2] = state[1][1];
            state[1][1] = state[1][0];
            state[1][0] = t;

            // Shift right by 2
            t = state[2][3];
            state[2][3] = state[2][1];
            state[2][1] = t;
            t = state[2][2];
            state[2][2] = state[2][0];
            state[2][0] = t;

            // Shift right by 3
            t = state[3][3];
            state[3][3] = state[3][0];
            state[3][0] = state[3][1];
            state[3][1] = state[3][2];
            state[3][2] = t;
        }

        private static void InvSubBytes(ref byte[][] state)
        {
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    state[i][j] = InvSBox[state[i][j] >> 4][state[i][j] & 0x0F];
                }
            }
        }

        /// <summary>
        /// Performs the MixColumns step. The state is multiplied by itself using matrix
        /// multiplication in a Galios Field 2^8. All multiplication is pre-computed in a table.
        /// Addition is equivalent to XOR. (Must always make a copy of the column as the original
        /// values will be destroyed.)
        /// </summary>
        /// <param name="state">The state for which to perform the mix columns step.</param>
        private static void MixColumns(ref byte[][] state)
        {
            var col = new byte[4];

            for (var i = 0; i < 4; ++i)
            {
                col[0] = state[0][i];
                col[1] = state[1][i];
                col[2] = state[2][i];
                col[3] = state[3][i];
                state[0][i] = GfMul[col[0]][0];
                state[0][i] ^= GfMul[col[1]][1];
                state[0][i] ^= col[2];
                state[0][i] ^= col[3];
                state[1][i] = col[0];
                state[1][i] ^= GfMul[col[1]][0];
                state[1][i] ^= GfMul[col[2]][1];
                state[1][i] ^= col[3];
                state[2][i] = col[0];
                state[2][i] ^= col[1];
                state[2][i] ^= GfMul[col[2]][0];
                state[2][i] ^= GfMul[col[3]][1];
                state[3][i] = GfMul[col[0]][1];
                state[3][i] ^= col[1];
                state[3][i] ^= col[2];
                state[3][i] ^= GfMul[col[3]][0];
            }
        }

        private static uint RotateWord(uint x)
        {
            return (x << 8) | (x >> 24);
        }

        /// <summary>
        /// Performs the ShiftRows step. All rows are shifted cylindrically to the left.
        /// </summary>
        /// <param name="state">The state for which to perform the shift-rows step.</param>
        private static void ShiftRows(ref byte[][] state)
        {
            // Shift left by 1
            var t = state[1][0];
            state[1][0] = state[1][1];
            state[1][1] = state[1][2];
            state[1][2] = state[1][3];
            state[1][3] = t;

            // Shift left by 2
            t = state[2][0];
            state[2][0] = state[2][2];
            state[2][2] = t;
            t = state[2][1];
            state[2][1] = state[2][3];
            state[2][3] = t;

            // Shift left by 3
            t = state[3][0];
            state[3][0] = state[3][3];
            state[3][3] = state[3][2];
            state[3][2] = state[3][1];
            state[3][1] = t;
        }

        /// <summary>
        /// Performs the SubBytes step. All bytes in the state are substituted with a
        /// pre-calculated value from a lookup table.
        /// </summary>
        /// <param name="state">The state for which to perform the sub-bytes step.</param>
        private static void SubBytes(ref byte[][] state)
        {
            for (var i = 0; i < 4; ++i)
            {
                for (var j = 0; j < 4; ++j)
                {
                    state[i][j] = SBox[state[i][j] >> 4][state[i][j] & 0x0F];
                }
            }
        }

        private static uint SubWord(uint word)
        {
            var result = (uint)SBox[(word >> 4) & 0x0000000F][word & 0x0000000F];
            result += (uint)SBox[(word >> 12) & 0x0000000F][(word >> 8) & 0x0000000F] << 8;
            result += (uint)SBox[(word >> 20) & 0x0000000F][(word >> 16) & 0x0000000F] << 16;
            result += (uint)SBox[(word >> 28) & 0x0000000F][(word >> 24) & 0x0000000F] << 24;
            return result;
        }

        /// <summary>
        /// XORs the in and out buffers, storing the result in out. Length is in bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="length">The length in bytes.</param>
        private static void XorBuf(IReadOnlyList<byte> input, ref byte[] output, int length)
        {
            for (var index = 0; index < length; index++)
            {
                output[index] ^= input[index];
            }
        }
    }
}