using QRCoder;
using QRCoder.Exceptions;
using System.Collections;

using System.Text;




namespace ApiForUrl.Methods;

public class QRCodeGenerator : IDisposable
{
    public enum EciMode
    {
        Default = 0,
        Iso8859_1 = 3,
        Iso8859_2 = 4,
        Utf8 = 26
    }

    private static class ModulePlacer
    {
        private static class MaskPattern
        {
            public static bool Pattern1(int x, int y)
            {
                return (x + y) % 2 == 0;
            }

            public static bool Pattern2(int x, int y)
            {
                return y % 2 == 0;
            }

            public static bool Pattern3(int x, int y)
            {
                return x % 3 == 0;
            }

            public static bool Pattern4(int x, int y)
            {
                return (x + y) % 3 == 0;
            }

            public static bool Pattern5(int x, int y)
            {
                return (int)(Math.Floor((double)y / 2.0) + Math.Floor((double)x / 3.0)) % 2 == 0;
            }

            public static bool Pattern6(int x, int y)
            {
                return x * y % 2 + x * y % 3 == 0;
            }

            public static bool Pattern7(int x, int y)
            {
                return (x * y % 2 + x * y % 3) % 2 == 0;
            }

            public static bool Pattern8(int x, int y)
            {
                return ((x + y) % 2 + x * y % 3) % 2 == 0;
            }

            public static int Score(ref QRCodeData qrCode)
            {
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int count = qrCode.ModuleMatrix.Count;
                for (int i = 0; i < count; i++)
                {
                    int num5 = 0;
                    int num6 = 0;
                    bool flag = qrCode.ModuleMatrix[i][0];
                    bool flag2 = qrCode.ModuleMatrix[0][i];
                    for (int j = 0; j < count; j++)
                    {
                        num5 = ((qrCode.ModuleMatrix[i][j] != flag) ? 1 : (num5 + 1));
                        if (num5 == 5)
                        {
                            num += 3;
                        }
                        else if (num5 > 5)
                        {
                            num++;
                        }

                        flag = qrCode.ModuleMatrix[i][j];
                        num6 = ((qrCode.ModuleMatrix[j][i] != flag2) ? 1 : (num6 + 1));
                        if (num6 == 5)
                        {
                            num += 3;
                        }
                        else if (num6 > 5)
                        {
                            num++;
                        }

                        flag2 = qrCode.ModuleMatrix[j][i];
                    }
                }

                for (int k = 0; k < count - 1; k++)
                {
                    for (int l = 0; l < count - 1; l++)
                    {
                        if (qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k][l + 1] && qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k + 1][l] && qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k + 1][l + 1])
                        {
                            num2 += 3;
                        }
                    }
                }

                for (int m = 0; m < count; m++)
                {
                    for (int n = 0; n < count - 10; n++)
                    {
                        if ((qrCode.ModuleMatrix[m][n] && !qrCode.ModuleMatrix[m][n + 1] && qrCode.ModuleMatrix[m][n + 2] && qrCode.ModuleMatrix[m][n + 3] && qrCode.ModuleMatrix[m][n + 4] && !qrCode.ModuleMatrix[m][n + 5] && qrCode.ModuleMatrix[m][n + 6] && !qrCode.ModuleMatrix[m][n + 7] && !qrCode.ModuleMatrix[m][n + 8] && !qrCode.ModuleMatrix[m][n + 9] && !qrCode.ModuleMatrix[m][n + 10]) || (!qrCode.ModuleMatrix[m][n] && !qrCode.ModuleMatrix[m][n + 1] && !qrCode.ModuleMatrix[m][n + 2] && !qrCode.ModuleMatrix[m][n + 3] && qrCode.ModuleMatrix[m][n + 4] && !qrCode.ModuleMatrix[m][n + 5] && qrCode.ModuleMatrix[m][n + 6] && qrCode.ModuleMatrix[m][n + 7] && qrCode.ModuleMatrix[m][n + 8] && !qrCode.ModuleMatrix[m][n + 9] && qrCode.ModuleMatrix[m][n + 10]))
                        {
                            num3 += 40;
                        }

                        if ((qrCode.ModuleMatrix[n][m] && !qrCode.ModuleMatrix[n + 1][m] && qrCode.ModuleMatrix[n + 2][m] && qrCode.ModuleMatrix[n + 3][m] && qrCode.ModuleMatrix[n + 4][m] && !qrCode.ModuleMatrix[n + 5][m] && qrCode.ModuleMatrix[n + 6][m] && !qrCode.ModuleMatrix[n + 7][m] && !qrCode.ModuleMatrix[n + 8][m] && !qrCode.ModuleMatrix[n + 9][m] && !qrCode.ModuleMatrix[n + 10][m]) || (!qrCode.ModuleMatrix[n][m] && !qrCode.ModuleMatrix[n + 1][m] && !qrCode.ModuleMatrix[n + 2][m] && !qrCode.ModuleMatrix[n + 3][m] && qrCode.ModuleMatrix[n + 4][m] && !qrCode.ModuleMatrix[n + 5][m] && qrCode.ModuleMatrix[n + 6][m] && qrCode.ModuleMatrix[n + 7][m] && qrCode.ModuleMatrix[n + 8][m] && !qrCode.ModuleMatrix[n + 9][m] && qrCode.ModuleMatrix[n + 10][m]))
                        {
                            num3 += 40;
                        }
                    }
                }

                double num7 = 0.0;
                foreach (BitArray item in qrCode.ModuleMatrix)
                {
                    foreach (bool item2 in item)
                    {
                        if (item2)
                        {
                            num7 += 1.0;
                        }
                    }
                }

                double num8 = num7 / (double)(qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count) * 100.0;
                int val = Math.Abs((int)Math.Floor(num8 / 5.0) * 5 - 50) / 5;
                int val2 = Math.Abs((int)Math.Floor(num8 / 5.0) * 5 - 45) / 5;
                num4 = Math.Min(val, val2) * 10;
                return num + num2 + num3 + num4;
            }
        }

        public static void AddQuietZone(ref QRCodeData qrCode)
        {
            bool[] array = new bool[qrCode.ModuleMatrix.Count + 8];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = false;
            }

            for (int j = 0; j < 4; j++)
            {
                qrCode.ModuleMatrix.Insert(0, new BitArray(array));
            }

            for (int k = 0; k < 4; k++)
            {
                qrCode.ModuleMatrix.Add(new BitArray(array));
            }

            for (int l = 4; l < qrCode.ModuleMatrix.Count - 4; l++)
            {
                bool[] collection = new bool[4];
                List<bool> list = new List<bool>(collection);
                list.AddRange(qrCode.ModuleMatrix[l].Cast<bool>());
                list.AddRange(collection);
                qrCode.ModuleMatrix[l] = new BitArray(list.ToArray());
            }
        }

        private static string ReverseString(string inp)
        {
            string text = string.Empty;
            if (inp.Length > 0)
            {
                for (int num = inp.Length - 1; num >= 0; num--)
                {
                    text += inp[num];
                }
            }

            return text;
        }

        public static void PlaceVersion(ref QRCodeData qrCode, string versionStr)
        {
            int count = qrCode.ModuleMatrix.Count;
            string text = ReverseString(versionStr);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    qrCode.ModuleMatrix[j + count - 11][i] = text[i * 3 + j] == '1';
                    qrCode.ModuleMatrix[i][j + count - 11] = text[i * 3 + j] == '1';
                }
            }
        }

        public static void PlaceFormat(ref QRCodeData qrCode, string formatStr)
        {
            int count = qrCode.ModuleMatrix.Count;
            string text = ReverseString(formatStr);
            int[,] obj = new int[15, 4]
            {
                { 8, 0, 0, 8 },
                { 8, 1, 0, 8 },
                { 8, 2, 0, 8 },
                { 8, 3, 0, 8 },
                { 8, 4, 0, 8 },
                { 8, 5, 0, 8 },
                { 8, 7, 0, 8 },
                { 8, 8, 0, 8 },
                { 7, 8, 8, 0 },
                { 5, 8, 8, 0 },
                { 4, 8, 8, 0 },
                { 3, 8, 8, 0 },
                { 2, 8, 8, 0 },
                { 1, 8, 8, 0 },
                { 0, 8, 8, 0 }
            };
            obj[0, 2] = count - 1;
            obj[1, 2] = count - 2;
            obj[2, 2] = count - 3;
            obj[3, 2] = count - 4;
            obj[4, 2] = count - 5;
            obj[5, 2] = count - 6;
            obj[6, 2] = count - 7;
            obj[7, 2] = count - 8;
            obj[8, 3] = count - 7;
            obj[9, 3] = count - 6;
            obj[10, 3] = count - 5;
            obj[11, 3] = count - 4;
            obj[12, 3] = count - 3;
            obj[13, 3] = count - 2;
            obj[14, 3] = count - 1;
            int[,] array = obj;
            for (int i = 0; i < 15; i++)
            {
                Point point = new Point(array[i, 0], array[i, 1]);
                Point point2 = new Point(array[i, 2], array[i, 3]);
                qrCode.ModuleMatrix[point.Y][point.X] = text[i] == '1';
                qrCode.ModuleMatrix[point2.Y][point2.X] = text[i] == '1';
            }
        }

        public static int MaskCode(ref QRCodeData qrCode, int version, ref List<Rectangle> blockedModules, ECCLevel eccLevel)
        {
            int? num = null;
            int num2 = 0;
            int count = qrCode.ModuleMatrix.Count;
            Dictionary<int, Func<int, int, bool>> dictionary = new Dictionary<int, Func<int, int, bool>>(8)
            {
                {
                    1,
                    MaskPattern.Pattern1
                },
                {
                    2,
                    MaskPattern.Pattern2
                },
                {
                    3,
                    MaskPattern.Pattern3
                },
                {
                    4,
                    MaskPattern.Pattern4
                },
                {
                    5,
                    MaskPattern.Pattern5
                },
                {
                    6,
                    MaskPattern.Pattern6
                },
                {
                    7,
                    MaskPattern.Pattern7
                },
                {
                    8,
                    MaskPattern.Pattern8
                }
            };
            foreach (KeyValuePair<int, Func<int, int, bool>> item in dictionary)
            {
                QRCodeData qrCode2 = new QRCodeData(version);
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        qrCode2.ModuleMatrix[i][j] = qrCode.ModuleMatrix[i][j];
                    }
                }

                string formatString = GetFormatString(eccLevel, item.Key - 1);
                PlaceFormat(ref qrCode2, formatString);
                if (version >= 7)
                {
                    string versionString = GetVersionString(version);
                    PlaceVersion(ref qrCode2, versionString);
                }

                for (int k = 0; k < count; k++)
                {
                    for (int l = 0; l < k; l++)
                    {
                        if (!IsBlocked(new Rectangle(k, l, 1, 1), blockedModules))
                        {
                            qrCode2.ModuleMatrix[l][k] ^= item.Value(k, l);
                            qrCode2.ModuleMatrix[k][l] ^= item.Value(l, k);
                        }
                    }

                    if (!IsBlocked(new Rectangle(k, k, 1, 1), blockedModules))
                    {
                        qrCode2.ModuleMatrix[k][k] ^= item.Value(k, k);
                    }
                }

                int num3 = MaskPattern.Score(ref qrCode2);
                if (!num.HasValue || num2 > num3)
                {
                    num = item.Key;
                    num2 = num3;
                }
            }

            for (int m = 0; m < count; m++)
            {
                for (int n = 0; n < m; n++)
                {
                    if (!IsBlocked(new Rectangle(m, n, 1, 1), blockedModules))
                    {
                        qrCode.ModuleMatrix[n][m] ^= dictionary[num.Value](m, n);
                        qrCode.ModuleMatrix[m][n] ^= dictionary[num.Value](n, m);
                    }
                }

                if (!IsBlocked(new Rectangle(m, m, 1, 1), blockedModules))
                {
                    qrCode.ModuleMatrix[m][m] ^= dictionary[num.Value](m, m);
                }
            }

            return num.Value - 1;
        }

        public static void PlaceDataWords(ref QRCodeData qrCode, string data, ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            bool flag = true;
            Queue<bool> queue = new Queue<bool>();
            for (int i = 0; i < data.Length; i++)
            {
                queue.Enqueue(data[i] != '0');
            }

            for (int num = count - 1; num >= 0; num -= 2)
            {
                if (num == 6)
                {
                    num = 5;
                }

                for (int j = 1; j <= count; j++)
                {
                    if (flag)
                    {
                        int num2 = count - j;
                        if (queue.Count > 0 && !IsBlocked(new Rectangle(num, num2, 1, 1), blockedModules))
                        {
                            qrCode.ModuleMatrix[num2][num] = queue.Dequeue();
                        }

                        if (queue.Count > 0 && num > 0 && !IsBlocked(new Rectangle(num - 1, num2, 1, 1), blockedModules))
                        {
                            qrCode.ModuleMatrix[num2][num - 1] = queue.Dequeue();
                        }
                    }
                    else
                    {
                        int num2 = j - 1;
                        if (queue.Count > 0 && !IsBlocked(new Rectangle(num, num2, 1, 1), blockedModules))
                        {
                            qrCode.ModuleMatrix[num2][num] = queue.Dequeue();
                        }

                        if (queue.Count > 0 && num > 0 && !IsBlocked(new Rectangle(num - 1, num2, 1, 1), blockedModules))
                        {
                            qrCode.ModuleMatrix[num2][num - 1] = queue.Dequeue();
                        }
                    }
                }

                flag = !flag;
            }
        }

        public static void ReserveSeperatorAreas(int size, ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange(new Rectangle[6]
            {
                new Rectangle(7, 0, 1, 8),
                new Rectangle(0, 7, 7, 1),
                new Rectangle(0, size - 8, 8, 1),
                new Rectangle(7, size - 7, 1, 7),
                new Rectangle(size - 8, 0, 1, 8),
                new Rectangle(size - 7, 7, 7, 1)
            });
        }

        public static void ReserveVersionAreas(int size, int version, ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange(new Rectangle[6]
            {
                new Rectangle(8, 0, 1, 6),
                new Rectangle(8, 7, 1, 1),
                new Rectangle(0, 8, 6, 1),
                new Rectangle(7, 8, 2, 1),
                new Rectangle(size - 8, 8, 8, 1),
                new Rectangle(8, size - 7, 1, 7)
            });
            if (version >= 7)
            {
                blockedModules.AddRange(new Rectangle[2]
                {
                    new Rectangle(size - 11, 0, 3, 6),
                    new Rectangle(0, size - 11, 6, 3)
                });
            }
        }

        public static void PlaceDarkModule(ref QRCodeData qrCode, int version, ref List<Rectangle> blockedModules)
        {
            qrCode.ModuleMatrix[4 * version + 9][8] = true;
            blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
        }

        public static void PlaceFinderPatterns(ref QRCodeData qrCode, ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            int[] array = new int[6]
            {
                0,
                0,
                count - 7,
                0,
                0,
                count - 7
            };
            for (int i = 0; i < 6; i += 2)
            {
                for (int j = 0; j < 7; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (((j != 1 && j != 5) || k <= 0 || k >= 6) && (j <= 0 || j >= 6 || (k != 1 && k != 5)))
                        {
                            qrCode.ModuleMatrix[k + array[i + 1]][j + array[i]] = true;
                        }
                    }
                }

                blockedModules.Add(new Rectangle(array[i], array[i + 1], 7, 7));
            }
        }

        public static void PlaceAlignmentPatterns(ref QRCodeData qrCode, List<Point> alignmentPatternLocations, ref List<Rectangle> blockedModules)
        {
            foreach (Point alignmentPatternLocation in alignmentPatternLocations)
            {
                Rectangle r = new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5);
                bool flag = false;
                foreach (Rectangle blockedModule in blockedModules)
                {
                    if (Intersects(r, blockedModule))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    continue;
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 0 || j == 4 || i == 0 || i == 4 || (i == 2 && j == 2))
                        {
                            qrCode.ModuleMatrix[alignmentPatternLocation.Y + j][alignmentPatternLocation.X + i] = true;
                        }
                    }
                }

                blockedModules.Add(new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5));
            }
        }

        public static void PlaceTimingPatterns(ref QRCodeData qrCode, ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            for (int i = 8; i < count - 8; i++)
            {
                if (i % 2 == 0)
                {
                    qrCode.ModuleMatrix[6][i] = true;
                    qrCode.ModuleMatrix[i][6] = true;
                }
            }

            blockedModules.AddRange(new Rectangle[2]
            {
                new Rectangle(6, 8, 1, count - 16),
                new Rectangle(8, 6, count - 16, 1)
            });
        }

        private static bool Intersects(Rectangle r1, Rectangle r2)
        {
            if (r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height)
            {
                return r1.Y < r2.Y + r2.Height;
            }

            return false;
        }

        private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
        {
            foreach (Rectangle blockedModule in blockedModules)
            {
                if (Intersects(blockedModule, r1))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum ECCLevel
    {
        L,
        M,
        Q,
        H
    }

    private enum EncodingMode
    {
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 4,
        Kanji = 8,
        ECI = 7
    }

    private struct AlignmentPattern
    {
        public int Version;

        public List<Point> PatternPositions;
    }

    private struct CodewordBlock
    {
        public int GroupNumber { get; }

        public int BlockNumber { get; }

        public string BitString { get; }

        public List<string> CodeWords { get; }

        public List<int> CodeWordsInt { get; }

        public List<string> ECCWords { get; }

        public List<int> ECCWordsInt { get; }

        public CodewordBlock(int groupNumber, int blockNumber, string bitString, List<string> codeWords, List<string> eccWords, List<int> codeWordsInt, List<int> eccWordsInt)
        {
            GroupNumber = groupNumber;
            BlockNumber = blockNumber;
            BitString = bitString;
            CodeWords = codeWords;
            ECCWords = eccWords;
            CodeWordsInt = codeWordsInt;
            ECCWordsInt = eccWordsInt;
        }
    }

    private struct ECCInfo
    {
        public int Version { get; }

        public ECCLevel ErrorCorrectionLevel { get; }

        public int TotalDataCodewords { get; }

        public int ECCPerBlock { get; }

        public int BlocksInGroup1 { get; }

        public int CodewordsInGroup1 { get; }

        public int BlocksInGroup2 { get; }

        public int CodewordsInGroup2 { get; }

        public ECCInfo(int version, ECCLevel errorCorrectionLevel, int totalDataCodewords, int eccPerBlock, int blocksInGroup1, int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
        {
            Version = version;
            ErrorCorrectionLevel = errorCorrectionLevel;
            TotalDataCodewords = totalDataCodewords;
            ECCPerBlock = eccPerBlock;
            BlocksInGroup1 = blocksInGroup1;
            CodewordsInGroup1 = codewordsInGroup1;
            BlocksInGroup2 = blocksInGroup2;
            CodewordsInGroup2 = codewordsInGroup2;
        }
    }

    private struct VersionInfo
    {
        public int Version { get; }

        public List<VersionInfoDetails> Details { get; }

        public VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
        {
            Version = version;
            Details = versionInfoDetails;
        }
    }

    private struct VersionInfoDetails
    {
        public ECCLevel ErrorCorrectionLevel { get; }

        public Dictionary<EncodingMode, int> CapacityDict { get; }

        public VersionInfoDetails(ECCLevel errorCorrectionLevel, Dictionary<EncodingMode, int> capacityDict)
        {
            ErrorCorrectionLevel = errorCorrectionLevel;
            CapacityDict = capacityDict;
        }
    }

    private struct Antilog
    {
        public int ExponentAlpha { get; }

        public int IntegerValue { get; }

        public Antilog(int exponentAlpha, int integerValue)
        {
            ExponentAlpha = exponentAlpha;
            IntegerValue = integerValue;
        }
    }

    private struct PolynomItem
    {
        public int Coefficient { get; }

        public int Exponent { get; }

        public PolynomItem(int coefficient, int exponent)
        {
            Coefficient = coefficient;
            Exponent = exponent;
        }
    }

    private class Polynom
    {
        public List<PolynomItem> PolyItems { get; set; }

        public Polynom()
        {
            PolyItems = new List<PolynomItem>();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PolynomItem polyItem in PolyItems)
            {
                stringBuilder.Append("a^" + polyItem.Coefficient + "*x^" + polyItem.Exponent + " + ");
            }

            return stringBuilder.ToString().TrimEnd(' ', '+');
        }
    }

    private class Point
    {
        public int X { get; }

        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private class Rectangle
    {
        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public Rectangle(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
    }

    private static readonly char[] alphanumEncTable = new char[9] { ' ', '$', '%', '*', '+', '-', '.', '/', ':' };

    private static readonly int[] capacityBaseValues = new int[640]
    {
        41, 25, 17, 10, 34, 20, 14, 8, 27, 16,
        11, 7, 17, 10, 7, 4, 77, 47, 32, 20,
        63, 38, 26, 16, 48, 29, 20, 12, 34, 20,
        14, 8, 127, 77, 53, 32, 101, 61, 42, 26,
        77, 47, 32, 20, 58, 35, 24, 15, 187, 114,
        78, 48, 149, 90, 62, 38, 111, 67, 46, 28,
        82, 50, 34, 21, 255, 154, 106, 65, 202, 122,
        84, 52, 144, 87, 60, 37, 106, 64, 44, 27,
        322, 195, 134, 82, 255, 154, 106, 65, 178, 108,
        74, 45, 139, 84, 58, 36, 370, 224, 154, 95,
        293, 178, 122, 75, 207, 125, 86, 53, 154, 93,
        64, 39, 461, 279, 192, 118, 365, 221, 152, 93,
        259, 157, 108, 66, 202, 122, 84, 52, 552, 335,
        230, 141, 432, 262, 180, 111, 312, 189, 130, 80,
        235, 143, 98, 60, 652, 395, 271, 167, 513, 311,
        213, 131, 364, 221, 151, 93, 288, 174, 119, 74,
        772, 468, 321, 198, 604, 366, 251, 155, 427, 259,
        177, 109, 331, 200, 137, 85, 883, 535, 367, 226,
        691, 419, 287, 177, 489, 296, 203, 125, 374, 227,
        155, 96, 1022, 619, 425, 262, 796, 483, 331, 204,
        580, 352, 241, 149, 427, 259, 177, 109, 1101, 667,
        458, 282, 871, 528, 362, 223, 621, 376, 258, 159,
        468, 283, 194, 120, 1250, 758, 520, 320, 991, 600,
        412, 254, 703, 426, 292, 180, 530, 321, 220, 136,
        1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470,
        322, 198, 602, 365, 250, 154, 1548, 938, 644, 397,
        1212, 734, 504, 310, 876, 531, 364, 224, 674, 408,
        280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345,
        948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153,
        792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272,
        813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970,
        666, 410, 1159, 702, 482, 297, 919, 557, 382, 235,
        2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742,
        509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618,
        1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640,
        439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528,
        1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704,
        1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407,
        1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451,
        997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330,
        3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094,
        751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902,
        2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910,
        625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732,
        2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369,
        1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559,
        1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994,
        1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457,
        4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499,
        1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201,
        3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226,
        842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002,
        2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183,
        2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756,
        2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632,
        1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605,
        5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966,
        1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577,
        4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591,
        1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292,
        3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087,
        2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972,
        2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391,
        2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784
    };

    private static readonly int[] capacityECCBaseValues = new int[960]
    {
        19, 7, 1, 19, 0, 0, 16, 10, 1, 16,
        0, 0, 13, 13, 1, 13, 0, 0, 9, 17,
        1, 9, 0, 0, 34, 10, 1, 34, 0, 0,
        28, 16, 1, 28, 0, 0, 22, 22, 1, 22,
        0, 0, 16, 28, 1, 16, 0, 0, 55, 15,
        1, 55, 0, 0, 44, 26, 1, 44, 0, 0,
        34, 18, 2, 17, 0, 0, 26, 22, 2, 13,
        0, 0, 80, 20, 1, 80, 0, 0, 64, 18,
        2, 32, 0, 0, 48, 26, 2, 24, 0, 0,
        36, 16, 4, 9, 0, 0, 108, 26, 1, 108,
        0, 0, 86, 24, 2, 43, 0, 0, 62, 18,
        2, 15, 2, 16, 46, 22, 2, 11, 2, 12,
        136, 18, 2, 68, 0, 0, 108, 16, 4, 27,
        0, 0, 76, 24, 4, 19, 0, 0, 60, 28,
        4, 15, 0, 0, 156, 20, 2, 78, 0, 0,
        124, 18, 4, 31, 0, 0, 88, 18, 2, 14,
        4, 15, 66, 26, 4, 13, 1, 14, 194, 24,
        2, 97, 0, 0, 154, 22, 2, 38, 2, 39,
        110, 22, 4, 18, 2, 19, 86, 26, 4, 14,
        2, 15, 232, 30, 2, 116, 0, 0, 182, 22,
        3, 36, 2, 37, 132, 20, 4, 16, 4, 17,
        100, 24, 4, 12, 4, 13, 274, 18, 2, 68,
        2, 69, 216, 26, 4, 43, 1, 44, 154, 24,
        6, 19, 2, 20, 122, 28, 6, 15, 2, 16,
        324, 20, 4, 81, 0, 0, 254, 30, 1, 50,
        4, 51, 180, 28, 4, 22, 4, 23, 140, 24,
        3, 12, 8, 13, 370, 24, 2, 92, 2, 93,
        290, 22, 6, 36, 2, 37, 206, 26, 4, 20,
        6, 21, 158, 28, 7, 14, 4, 15, 428, 26,
        4, 107, 0, 0, 334, 22, 8, 37, 1, 38,
        244, 24, 8, 20, 4, 21, 180, 22, 12, 11,
        4, 12, 461, 30, 3, 115, 1, 116, 365, 24,
        4, 40, 5, 41, 261, 20, 11, 16, 5, 17,
        197, 24, 11, 12, 5, 13, 523, 22, 5, 87,
        1, 88, 415, 24, 5, 41, 5, 42, 295, 30,
        5, 24, 7, 25, 223, 24, 11, 12, 7, 13,
        589, 24, 5, 98, 1, 99, 453, 28, 7, 45,
        3, 46, 325, 24, 15, 19, 2, 20, 253, 30,
        3, 15, 13, 16, 647, 28, 1, 107, 5, 108,
        507, 28, 10, 46, 1, 47, 367, 28, 1, 22,
        15, 23, 283, 28, 2, 14, 17, 15, 721, 30,
        5, 120, 1, 121, 563, 26, 9, 43, 4, 44,
        397, 28, 17, 22, 1, 23, 313, 28, 2, 14,
        19, 15, 795, 28, 3, 113, 4, 114, 627, 26,
        3, 44, 11, 45, 445, 26, 17, 21, 4, 22,
        341, 26, 9, 13, 16, 14, 861, 28, 3, 107,
        5, 108, 669, 26, 3, 41, 13, 42, 485, 30,
        15, 24, 5, 25, 385, 28, 15, 15, 10, 16,
        932, 28, 4, 116, 4, 117, 714, 26, 17, 42,
        0, 0, 512, 28, 17, 22, 6, 23, 406, 30,
        19, 16, 6, 17, 1006, 28, 2, 111, 7, 112,
        782, 28, 17, 46, 0, 0, 568, 30, 7, 24,
        16, 25, 442, 24, 34, 13, 0, 0, 1094, 30,
        4, 121, 5, 122, 860, 28, 4, 47, 14, 48,
        614, 30, 11, 24, 14, 25, 464, 30, 16, 15,
        14, 16, 1174, 30, 6, 117, 4, 118, 914, 28,
        6, 45, 14, 46, 664, 30, 11, 24, 16, 25,
        514, 30, 30, 16, 2, 17, 1276, 26, 8, 106,
        4, 107, 1000, 28, 8, 47, 13, 48, 718, 30,
        7, 24, 22, 25, 538, 30, 22, 15, 13, 16,
        1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46,
        4, 47, 754, 28, 28, 22, 6, 23, 596, 30,
        33, 16, 4, 17, 1468, 30, 8, 122, 4, 123,
        1128, 28, 22, 45, 3, 46, 808, 30, 8, 23,
        26, 24, 628, 30, 12, 15, 28, 16, 1531, 30,
        3, 117, 10, 118, 1193, 28, 3, 45, 23, 46,
        871, 30, 4, 24, 31, 25, 661, 30, 11, 15,
        31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28,
        21, 45, 7, 46, 911, 30, 1, 23, 37, 24,
        701, 30, 19, 15, 26, 16, 1735, 30, 5, 115,
        10, 116, 1373, 28, 19, 47, 10, 48, 985, 30,
        15, 24, 25, 25, 745, 30, 23, 15, 25, 16,
        1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46,
        29, 47, 1033, 30, 42, 24, 1, 25, 793, 30,
        23, 15, 28, 16, 1955, 30, 17, 115, 0, 0,
        1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24,
        35, 25, 845, 30, 19, 15, 35, 16, 2071, 30,
        17, 115, 1, 116, 1631, 28, 14, 46, 21, 47,
        1171, 30, 29, 24, 19, 25, 901, 30, 11, 15,
        46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28,
        14, 46, 23, 47, 1231, 30, 44, 24, 7, 25,
        961, 30, 59, 16, 1, 17, 2306, 30, 12, 121,
        7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30,
        39, 24, 14, 25, 986, 30, 22, 15, 41, 16,
        2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47,
        34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30,
        2, 15, 64, 16, 2566, 30, 17, 122, 4, 123,
        1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24,
        10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30,
        4, 122, 18, 123, 2102, 28, 13, 46, 32, 47,
        1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15,
        32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28,
        40, 47, 7, 48, 1582, 30, 43, 24, 22, 25,
        1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118,
        6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30,
        34, 24, 34, 25, 1276, 30, 20, 15, 61, 16
    };

    private static readonly int[] alignmentPatternBaseValues = new int[280]
    {
        0, 0, 0, 0, 0, 0, 0, 6, 18, 0,
        0, 0, 0, 0, 6, 22, 0, 0, 0, 0,
        0, 6, 26, 0, 0, 0, 0, 0, 6, 30,
        0, 0, 0, 0, 0, 6, 34, 0, 0, 0,
        0, 0, 6, 22, 38, 0, 0, 0, 0, 6,
        24, 42, 0, 0, 0, 0, 6, 26, 46, 0,
        0, 0, 0, 6, 28, 50, 0, 0, 0, 0,
        6, 30, 54, 0, 0, 0, 0, 6, 32, 58,
        0, 0, 0, 0, 6, 34, 62, 0, 0, 0,
        0, 6, 26, 46, 66, 0, 0, 0, 6, 26,
        48, 70, 0, 0, 0, 6, 26, 50, 74, 0,
        0, 0, 6, 30, 54, 78, 0, 0, 0, 6,
        30, 56, 82, 0, 0, 0, 6, 30, 58, 86,
        0, 0, 0, 6, 34, 62, 90, 0, 0, 0,
        6, 28, 50, 72, 94, 0, 0, 6, 26, 50,
        74, 98, 0, 0, 6, 30, 54, 78, 102, 0,
        0, 6, 28, 54, 80, 106, 0, 0, 6, 32,
        58, 84, 110, 0, 0, 6, 30, 58, 86, 114,
        0, 0, 6, 34, 62, 90, 118, 0, 0, 6,
        26, 50, 74, 98, 122, 0, 6, 30, 54, 78,
        102, 126, 0, 6, 26, 52, 78, 104, 130, 0,
        6, 30, 56, 82, 108, 134, 0, 6, 34, 60,
        86, 112, 138, 0, 6, 30, 58, 86, 114, 142,
        0, 6, 34, 62, 90, 118, 146, 0, 6, 30,
        54, 78, 102, 126, 150, 6, 24, 50, 76, 102,
        128, 154, 6, 28, 54, 80, 106, 132, 158, 6,
        32, 58, 84, 110, 136, 162, 6, 26, 54, 82,
        110, 138, 166, 6, 30, 58, 86, 114, 142, 170
    };

    private static readonly int[] remainderBits = new int[40]
    {
        0, 7, 7, 7, 7, 7, 0, 0, 0, 0,
        0, 0, 0, 3, 3, 3, 3, 3, 3, 3,
        4, 4, 4, 4, 4, 4, 4, 3, 3, 3,
        3, 3, 3, 3, 0, 0, 0, 0, 0, 0
    };

    private static readonly List<AlignmentPattern> alignmentPatternTable = CreateAlignmentPatternTable();

    private static readonly List<ECCInfo> capacityECCTable = CreateCapacityECCTable();

    private static readonly List<VersionInfo> capacityTable = CreateCapacityTable();

    private static readonly List<Antilog> galoisField = CreateAntilogTable();

    private static readonly Dictionary<char, int> alphanumEncDict = CreateAlphanumEncDict();



    public QRCodeData CreateQrCode(PayloadGenerator.Payload payload)
    {
        return GenerateQrCode(payload);
    }

    public QRCodeData CreateQrCode(PayloadGenerator.Payload payload, ECCLevel eccLevel)
    {
        return GenerateQrCode(payload, eccLevel);
    }

    public QRCodeData CreateQrCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1)
    {
        return GenerateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
    }

    public QRCodeData CreateQrCode(byte[] binaryData, ECCLevel eccLevel)
    {
        return GenerateQrCode(binaryData, eccLevel);
    }

    public static QRCodeData GenerateQrCode(PayloadGenerator.Payload payload)
    {
        return GenerateQrCode(payload.ToString(), payload.EccLevel, forceUtf8: false, utf8BOM: false, payload.EciMode, payload.Version);
    }

    public static QRCodeData GenerateQrCode(PayloadGenerator.Payload payload, ECCLevel eccLevel)
    {
        return GenerateQrCode(payload.ToString(), eccLevel, forceUtf8: false, utf8BOM: false, payload.EciMode, payload.Version);
    }

    public static QRCodeData GenerateQrCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1)
    {
        EncodingMode encodingFromPlaintext = GetEncodingFromPlaintext(plainText, forceUtf8);
        string text = PlainTextToBinary(plainText, encodingFromPlaintext, eciMode, utf8BOM, forceUtf8);
        int dataLength = GetDataLength(encodingFromPlaintext, plainText, text, forceUtf8);
        int num = requestedVersion;
        if (num == -1)
        {
            num = GetVersion(dataLength + ((eciMode != 0) ? 2 : 0), encodingFromPlaintext, eccLevel);
        }
        else if (GetVersion(dataLength + ((eciMode != 0) ? 2 : 0), encodingFromPlaintext, eccLevel) > num)
        {
            int maxSizeByte = capacityTable[num - 1].Details.First((VersionInfoDetails x) => x.ErrorCorrectionLevel == eccLevel).CapacityDict[encodingFromPlaintext];
            throw new DataTooLongException(eccLevel.ToString(), encodingFromPlaintext.ToString(), num, maxSizeByte);
        }

        string text2 = string.Empty;
        if (eciMode != 0)
        {
            text2 = DecToBin(7, 4);
            text2 += DecToBin((int)eciMode, 8);
        }

        text2 += DecToBin((int)encodingFromPlaintext, 4);
        string text3 = DecToBin(dataLength, GetCountIndicatorLength(num, encodingFromPlaintext));
        return GenerateQrCode(string.Concat(text2 + text3, text), eccLevel, num);
    }

    public static QRCodeData GenerateQrCode(byte[] binaryData, ECCLevel eccLevel)
    {
        int version = GetVersion(binaryData.Length, EncodingMode.Byte, eccLevel);
        string text = DecToBin(4, 4);
        string text2 = DecToBin(binaryData.Length, GetCountIndicatorLength(version, EncodingMode.Byte));
        string text3 = text + text2;
        foreach (byte decNum in binaryData)
        {
            text3 += DecToBin(decNum, 8);
        }

        return GenerateQrCode(text3, eccLevel, version);
    }

    private static QRCodeData GenerateQrCode(string bitString, ECCLevel eccLevel, int version)
    {
        ECCInfo eccInfo = capacityECCTable.Single((ECCInfo x) => x.Version == version && x.ErrorCorrectionLevel == eccLevel);
        int num = eccInfo.TotalDataCodewords * 8;
        int num2 = num - bitString.Length;
        if (num2 > 0)
        {
            bitString += new string('0', Math.Min(num2, 4));
        }

        if (bitString.Length % 8 != 0)
        {
            bitString += new string('0', 8 - bitString.Length % 8);
        }

        while (bitString.Length < num)
        {
            bitString += "1110110000010001";
        }

        if (bitString.Length > num)
        {
            bitString = bitString.Substring(0, num);
        }

        List<CodewordBlock> list = new List<CodewordBlock>(eccInfo.BlocksInGroup1 + eccInfo.BlocksInGroup2);
        for (int i = 0; i < eccInfo.BlocksInGroup1; i++)
        {
            string bitString2 = bitString.Substring(i * eccInfo.CodewordsInGroup1 * 8, eccInfo.CodewordsInGroup1 * 8);
            List<string> list2 = BinaryStringToBitBlockList(bitString2);
            List<int> codeWordsInt = BinaryStringListToDecList(list2);
            List<string> list3 = CalculateECCWords(bitString2, eccInfo);
            List<int> eccWordsInt = BinaryStringListToDecList(list3);
            list.Add(new CodewordBlock(1, i + 1, bitString2, list2, list3, codeWordsInt, eccWordsInt));
        }

        bitString = bitString.Substring(eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8);
        for (int j = 0; j < eccInfo.BlocksInGroup2; j++)
        {
            string bitString3 = bitString.Substring(j * eccInfo.CodewordsInGroup2 * 8, eccInfo.CodewordsInGroup2 * 8);
            List<string> list4 = BinaryStringToBitBlockList(bitString3);
            List<int> codeWordsInt2 = BinaryStringListToDecList(list4);
            List<string> list5 = CalculateECCWords(bitString3, eccInfo);
            List<int> eccWordsInt2 = BinaryStringListToDecList(list5);
            list.Add(new CodewordBlock(2, j + 1, bitString3, list4, list5, codeWordsInt2, eccWordsInt2));
        }

        StringBuilder stringBuilder = new StringBuilder();
        for (int k = 0; k < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); k++)
        {
            foreach (CodewordBlock item in list)
            {
                if (item.CodeWords.Count > k)
                {
                    stringBuilder.Append(item.CodeWords[k]);
                }
            }
        }

        for (int l = 0; l < eccInfo.ECCPerBlock; l++)
        {
            foreach (CodewordBlock item2 in list)
            {
                if (item2.ECCWords.Count > l)
                {
                    stringBuilder.Append(item2.ECCWords[l]);
                }
            }
        }

        stringBuilder.Append(new string('0', remainderBits[version - 1]));
        string data = stringBuilder.ToString();
        QRCodeData qrCode = new QRCodeData(version);
        List<Rectangle> blockedModules = new List<Rectangle>();
        ModulePlacer.PlaceFinderPatterns(ref qrCode, ref blockedModules);
        ModulePlacer.ReserveSeperatorAreas(qrCode.ModuleMatrix.Count, ref blockedModules);
        ModulePlacer.PlaceAlignmentPatterns(ref qrCode, (from x in alignmentPatternTable
                                                         where x.Version == version
                                                         select x.PatternPositions).First(), ref blockedModules);
        ModulePlacer.PlaceTimingPatterns(ref qrCode, ref blockedModules);
        ModulePlacer.PlaceDarkModule(ref qrCode, version, ref blockedModules);
        ModulePlacer.ReserveVersionAreas(qrCode.ModuleMatrix.Count, version, ref blockedModules);
        ModulePlacer.PlaceDataWords(ref qrCode, data, ref blockedModules);
        int maskVersion = ModulePlacer.MaskCode(ref qrCode, version, ref blockedModules, eccLevel);
        string formatString = GetFormatString(eccLevel, maskVersion);
        ModulePlacer.PlaceFormat(ref qrCode, formatString);
        if (version >= 7)
        {
            string versionString = GetVersionString(version);
            ModulePlacer.PlaceVersion(ref qrCode, versionString);
        }

        ModulePlacer.AddQuietZone(ref qrCode);
        return qrCode;
    }

    private static string GetFormatString(ECCLevel level, int maskVersion)
    {
        string text = "10100110111";
        string text2 = "101010000010010";
        string text3 = level switch
        {
            ECCLevel.Q => "11",
            ECCLevel.M => "00",
            ECCLevel.L => "01",
            _ => "10",
        } + DecToBin(maskVersion, 3);
        string text4 = text3.PadRight(15, '0').TrimStart('0');
        while (text4.Length > 10)
        {
            StringBuilder stringBuilder = new StringBuilder();
            text = text.PadRight(text4.Length, '0');
            for (int i = 0; i < text4.Length; i++)
            {
                stringBuilder.Append((Convert.ToInt32(text4[i]) ^ Convert.ToInt32(text[i])).ToString());
            }

            text4 = stringBuilder.ToString().TrimStart('0');
        }

        text4 = text4.PadLeft(10, '0');
        text3 += text4;
        StringBuilder stringBuilder2 = new StringBuilder();
        for (int j = 0; j < text3.Length; j++)
        {
            stringBuilder2.Append((Convert.ToInt32(text3[j]) ^ Convert.ToInt32(text2[j])).ToString());
        }

        return stringBuilder2.ToString();
    }

    private static string GetVersionString(int version)
    {
        string text = "1111100100101";
        string text2 = DecToBin(version, 6);
        string text3 = text2.PadRight(18, '0').TrimStart('0');
        while (text3.Length > 12)
        {
            StringBuilder stringBuilder = new StringBuilder();
            text = text.PadRight(text3.Length, '0');
            for (int i = 0; i < text3.Length; i++)
            {
                stringBuilder.Append((Convert.ToInt32(text3[i]) ^ Convert.ToInt32(text[i])).ToString());
            }

            text3 = stringBuilder.ToString().TrimStart('0');
        }

        text3 = text3.PadLeft(12, '0');
        return text2 + text3;
    }

    private static List<string> CalculateECCWords(string bitString, ECCInfo eccInfo)
    {
        int eCCPerBlock = eccInfo.ECCPerBlock;
        Polynom polynom = CalculateMessagePolynom(bitString);
        Polynom polynom2 = CalculateGeneratorPolynom(eCCPerBlock);
        for (int i = 0; i < polynom.PolyItems.Count; i++)
        {
            polynom.PolyItems[i] = new PolynomItem(polynom.PolyItems[i].Coefficient, polynom.PolyItems[i].Exponent + eCCPerBlock);
        }

        for (int j = 0; j < polynom2.PolyItems.Count; j++)
        {
            polynom2.PolyItems[j] = new PolynomItem(polynom2.PolyItems[j].Coefficient, polynom2.PolyItems[j].Exponent + (polynom.PolyItems.Count - 1));
        }

        Polynom polynom3 = polynom;
        int num = 0;
        while (polynom3.PolyItems.Count > 0 && polynom3.PolyItems[polynom3.PolyItems.Count - 1].Exponent > 0)
        {
            if (polynom3.PolyItems[0].Coefficient == 0)
            {
                polynom3.PolyItems.RemoveAt(0);
                polynom3.PolyItems.Add(new PolynomItem(0, polynom3.PolyItems[polynom3.PolyItems.Count - 1].Exponent - 1));
            }
            else
            {
                Polynom poly = MultiplyGeneratorPolynomByLeadterm(polynom2, ConvertToAlphaNotation(polynom3).PolyItems[0], num);
                poly = ConvertToDecNotation(poly);
                poly = XORPolynoms(polynom3, poly);
                polynom3 = poly;
            }

            num++;
        }

        return polynom3.PolyItems.Select((PolynomItem x) => DecToBin(x.Coefficient, 8)).ToList();
    }

    private static Polynom ConvertToAlphaNotation(Polynom poly)
    {
        Polynom polynom = new Polynom();
        for (int i = 0; i < poly.PolyItems.Count; i++)
        {
            polynom.PolyItems.Add(new PolynomItem((poly.PolyItems[i].Coefficient != 0) ? GetAlphaExpFromIntVal(poly.PolyItems[i].Coefficient) : 0, poly.PolyItems[i].Exponent));
        }

        return polynom;
    }

    private static Polynom ConvertToDecNotation(Polynom poly)
    {
        Polynom polynom = new Polynom();
        for (int i = 0; i < poly.PolyItems.Count; i++)
        {
            polynom.PolyItems.Add(new PolynomItem(GetIntValFromAlphaExp(poly.PolyItems[i].Coefficient), poly.PolyItems[i].Exponent));
        }

        return polynom;
    }

    private static int GetVersion(int length, EncodingMode encMode, ECCLevel eccLevel)
    {
        var source = from x in capacityTable
                     where x.Details.Any((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel && y.CapacityDict[encMode] >= Convert.ToInt32(length))
                     select new
                     {
                         version = x.Version,
                         capacity = x.Details.Single((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]
                     };
        if (source.Any())
        {
            return source.Min(x => x.version);
        }

        int maxSizeByte = capacityTable.Where((VersionInfo x) => x.Details.Any((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel)).Max((VersionInfo x) => x.Details.Single((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]);
        throw new DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
    }

    private static EncodingMode GetEncodingFromPlaintext(string plainText, bool forceUtf8)
    {
        if (forceUtf8)
        {
            return EncodingMode.Byte;
        }

        EncodingMode result = EncodingMode.Numeric;
        foreach (char c in plainText)
        {
            if (!IsInRange(c, '0', '9'))
            {
                result = EncodingMode.Alphanumeric;
                if (!IsInRange(c, 'A', 'Z') && !alphanumEncTable.Contains(c))
                {
                    return EncodingMode.Byte;
                }
            }
        }

        return result;
    }

    private static bool IsInRange(char c, char min, char max)
    {
        return (uint)(c - min) <= (uint)(max - min);
    }

    private static Polynom CalculateMessagePolynom(string bitString)
    {
        Polynom polynom = new Polynom();
        for (int num = bitString.Length / 8 - 1; num >= 0; num--)
        {
            polynom.PolyItems.Add(new PolynomItem(BinToDec(bitString.Substring(0, 8)), num));
            bitString = bitString.Remove(0, 8);
        }

        return polynom;
    }

    private static Polynom CalculateGeneratorPolynom(int numEccWords)
    {
        Polynom polynom = new Polynom();
        polynom.PolyItems.AddRange(new PolynomItem[2]
        {
            new PolynomItem(0, 1),
            new PolynomItem(0, 0)
        });
        for (int i = 1; i <= numEccWords - 1; i++)
        {
            Polynom polynom2 = new Polynom();
            polynom2.PolyItems.AddRange(new PolynomItem[2]
            {
                new PolynomItem(0, 1),
                new PolynomItem(i, 0)
            });
            polynom = MultiplyAlphaPolynoms(polynom, polynom2);
        }

        return polynom;
    }

    private static List<string> BinaryStringToBitBlockList(string bitString)
    {
        List<string> list = new List<string>((int)Math.Ceiling((double)bitString.Length / 8.0));
        for (int i = 0; i < bitString.Length; i += 8)
        {
            list.Add(bitString.Substring(i, 8));
        }

        return list;
    }

    private static List<int> BinaryStringListToDecList(List<string> binaryStringList)
    {
        return binaryStringList.Select((string binaryString) => BinToDec(binaryString)).ToList();
    }

    private static int BinToDec(string binStr)
    {
        return Convert.ToInt32(binStr, 2);
    }

    private static string DecToBin(int decNum)
    {
        return Convert.ToString(decNum, 2);
    }

    private static string DecToBin(int decNum, int padLeftUpTo)
    {
        return DecToBin(decNum).PadLeft(padLeftUpTo, '0');
    }

    private static int GetCountIndicatorLength(int version, EncodingMode encMode)
    {
        if (version < 10)
        {
            return encMode switch
            {
                EncodingMode.Numeric => 10,
                EncodingMode.Alphanumeric => 9,
                _ => 8,
            };
        }

        if (version < 27)
        {
            return encMode switch
            {
                EncodingMode.Numeric => 12,
                EncodingMode.Alphanumeric => 11,
                EncodingMode.Byte => 16,
                _ => 10,
            };
        }

        return encMode switch
        {
            EncodingMode.Numeric => 14,
            EncodingMode.Alphanumeric => 13,
            EncodingMode.Byte => 16,
            _ => 12,
        };
    }

    private static int GetDataLength(EncodingMode encoding, string plainText, string codedText, bool forceUtf8)
    {
        if (!forceUtf8 && !IsUtf8(encoding, plainText, forceUtf8))
        {
            return plainText.Length;
        }

        return codedText.Length / 8;
    }

    private static bool IsUtf8(EncodingMode encoding, string plainText, bool forceUtf8)
    {
        if (encoding == EncodingMode.Byte)
        {
            return !IsValidISO(plainText) || forceUtf8;
        }

        return false;
    }

    private static bool IsValidISO(string input)
    {
        byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
        string @string = Encoding.GetEncoding("ISO-8859-1").GetString(bytes, 0, bytes.Length);
        return string.Equals(input, @string);
    }

    private static string PlainTextToBinary(string plainText, EncodingMode encMode, EciMode eciMode, bool utf8BOM, bool forceUtf8)
    {
        return encMode switch
        {
            EncodingMode.Alphanumeric => PlainTextToBinaryAlphanumeric(plainText),
            EncodingMode.Numeric => PlainTextToBinaryNumeric(plainText),
            EncodingMode.Byte => PlainTextToBinaryByte(plainText, eciMode, utf8BOM, forceUtf8),
            EncodingMode.Kanji => string.Empty,
            _ => string.Empty,
        };
    }

    private static string PlainTextToBinaryNumeric(string plainText)
    {
        string text = string.Empty;
        while (plainText.Length >= 3)
        {
            int decNum = Convert.ToInt32(plainText.Substring(0, 3));
            text += DecToBin(decNum, 10);
            plainText = plainText.Substring(3);
        }

        if (plainText.Length == 2)
        {
            int decNum2 = Convert.ToInt32(plainText);
            text += DecToBin(decNum2, 7);
        }
        else if (plainText.Length == 1)
        {
            int decNum3 = Convert.ToInt32(plainText);
            text += DecToBin(decNum3, 4);
        }

        return text;
    }

    private static string PlainTextToBinaryAlphanumeric(string plainText)
    {
        string text = string.Empty;
        while (plainText.Length >= 2)
        {
            string text2 = plainText.Substring(0, 2);
            int decNum = alphanumEncDict[text2[0]] * 45 + alphanumEncDict[text2[1]];
            text += DecToBin(decNum, 11);
            plainText = plainText.Substring(2);
        }

        if (plainText.Length > 0)
        {
            text += DecToBin(alphanumEncDict[plainText[0]], 6);
        }

        return text;
    }

    private string PlainTextToBinaryECI(string plainText)
    {
        string text = string.Empty;
        byte[] bytes = Encoding.GetEncoding("ascii").GetBytes(plainText);
        foreach (byte decNum in bytes)
        {
            text += DecToBin(decNum, 8);
        }

        return text;
    }

    private static string ConvertToIso8859(string value, string Iso = "ISO-8859-2")
    {
        Encoding encoding = Encoding.GetEncoding(Iso);
        Encoding uTF = Encoding.UTF8;
        byte[] bytes = uTF.GetBytes(value);
        byte[] bytes2 = Encoding.Convert(uTF, encoding, bytes);
        return encoding.GetString(bytes2);
    }

    private static string PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
    {
        string text = string.Empty;
        byte[] array = ((IsValidISO(plainText) && !forceUtf8) ? Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText) : (eciMode switch
        {
            EciMode.Iso8859_1 => Encoding.GetEncoding("ISO-8859-1").GetBytes(ConvertToIso8859(plainText, "ISO-8859-1")),
            EciMode.Iso8859_2 => Encoding.GetEncoding("ISO-8859-2").GetBytes(ConvertToIso8859(plainText)),
            _ => utf8BOM ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(plainText)).ToArray() : Encoding.UTF8.GetBytes(plainText),
        }));
        byte[] array2 = array;
        foreach (byte decNum in array2)
        {
            text += DecToBin(decNum, 8);
        }

        return text;
    }

    private static Polynom XORPolynoms(Polynom messagePolynom, Polynom resPolynom)
    {
        Polynom polynom = new Polynom();
        Polynom polynom2;
        Polynom polynom3;
        if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
        {
            polynom2 = messagePolynom;
            polynom3 = resPolynom;
        }
        else
        {
            polynom2 = resPolynom;
            polynom3 = messagePolynom;
        }

        for (int i = 0; i < polynom2.PolyItems.Count; i++)
        {
            PolynomItem item = new PolynomItem(polynom2.PolyItems[i].Coefficient ^ ((polynom3.PolyItems.Count > i) ? polynom3.PolyItems[i].Coefficient : 0), messagePolynom.PolyItems[0].Exponent - i);
            polynom.PolyItems.Add(item);
        }

        polynom.PolyItems.RemoveAt(0);
        return polynom;
    }

    private static Polynom MultiplyGeneratorPolynomByLeadterm(Polynom genPolynom, PolynomItem leadTerm, int lowerExponentBy)
    {
        Polynom polynom = new Polynom();
        foreach (PolynomItem polyItem in genPolynom.PolyItems)
        {
            PolynomItem item = new PolynomItem((polyItem.Coefficient + leadTerm.Coefficient) % 255, polyItem.Exponent - lowerExponentBy);
            polynom.PolyItems.Add(item);
        }

        return polynom;
    }

    private static Polynom MultiplyAlphaPolynoms(Polynom polynomBase, Polynom polynomMultiplier)
    {
        Polynom polynom = new Polynom();
        foreach (PolynomItem polyItem in polynomMultiplier.PolyItems)
        {
            foreach (PolynomItem polyItem2 in polynomBase.PolyItems)
            {
                PolynomItem item = new PolynomItem(ShrinkAlphaExp(polyItem.Coefficient + polyItem2.Coefficient), polyItem.Exponent + polyItem2.Exponent);
                polynom.PolyItems.Add(item);
            }
        }

        IEnumerable<int> enumerable = from x in polynom.PolyItems
                                      group x by x.Exponent into x
                                      where x.Count() > 1
                                      select x.First().Exponent;
        IList<int> toGlue = (enumerable as IList<int>) ?? enumerable.ToList();
        List<PolynomItem> list = new List<PolynomItem>(toGlue.Count);
        foreach (int exponent in toGlue)
        {
            int intVal = polynom.PolyItems.Where((PolynomItem x) => x.Exponent == exponent).Aggregate(0, (int current, PolynomItem polynomOld) => current ^ GetIntValFromAlphaExp(polynomOld.Coefficient));
            PolynomItem item2 = new PolynomItem(GetAlphaExpFromIntVal(intVal), exponent);
            list.Add(item2);
        }

        polynom.PolyItems.RemoveAll((PolynomItem x) => toGlue.Contains(x.Exponent));
        polynom.PolyItems.AddRange(list);
        polynom.PolyItems.Sort((PolynomItem x, PolynomItem y) => -x.Exponent.CompareTo(y.Exponent));
        return polynom;
    }

    private static int GetIntValFromAlphaExp(int exp)
    {
        return galoisField.Find((Antilog alog) => alog.ExponentAlpha == exp).IntegerValue;
    }

    private static int GetAlphaExpFromIntVal(int intVal)
    {
        return galoisField.Find((Antilog alog) => alog.IntegerValue == intVal).ExponentAlpha;
    }

    private static int ShrinkAlphaExp(int alphaExp)
    {
        return (int)((double)(alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
    }

    private static Dictionary<char, int> CreateAlphanumEncDict()
    {
        Dictionary<char, int> dictionary = new Dictionary<char, int>(45);
        for (int i = 0; i < 10; i++)
        {
            dictionary.Add($"{i}"[0], i);
        }

        for (char c = 'A'; c <= 'Z'; c = (char)(c + 1))
        {
            dictionary.Add(c, dictionary.Count());
        }

        for (int j = 0; j < alphanumEncTable.Length; j++)
        {
            dictionary.Add(alphanumEncTable[j], dictionary.Count());
        }

        return dictionary;
    }

    private static List<AlignmentPattern> CreateAlignmentPatternTable()
    {
        List<AlignmentPattern> list = new List<AlignmentPattern>(40);
        for (int i = 0; i < 280; i += 7)
        {
            List<Point> list2 = new List<Point>();
            for (int j = 0; j < 7; j++)
            {
                if (alignmentPatternBaseValues[i + j] == 0)
                {
                    continue;
                }

                for (int k = 0; k < 7; k++)
                {
                    if (alignmentPatternBaseValues[i + k] != 0)
                    {
                        Point item = new Point(alignmentPatternBaseValues[i + j] - 2, alignmentPatternBaseValues[i + k] - 2);
                        if (!list2.Contains(item))
                        {
                            list2.Add(item);
                        }
                    }
                }
            }

            list.Add(new AlignmentPattern
            {
                Version = (i + 7) / 7,
                PatternPositions = list2
            });
        }

        return list;
    }

    private static List<ECCInfo> CreateCapacityECCTable()
    {
        List<ECCInfo> list = new List<ECCInfo>(160);
        for (int i = 0; i < 960; i += 24)
        {
            list.AddRange(new ECCInfo[4]
            {
                new ECCInfo((i + 24) / 24, ECCLevel.L, capacityECCBaseValues[i], capacityECCBaseValues[i + 1], capacityECCBaseValues[i + 2], capacityECCBaseValues[i + 3], capacityECCBaseValues[i + 4], capacityECCBaseValues[i + 5]),
                new ECCInfo((i + 24) / 24, ECCLevel.M, capacityECCBaseValues[i + 6], capacityECCBaseValues[i + 7], capacityECCBaseValues[i + 8], capacityECCBaseValues[i + 9], capacityECCBaseValues[i + 10], capacityECCBaseValues[i + 11]),
                new ECCInfo((i + 24) / 24, ECCLevel.Q, capacityECCBaseValues[i + 12], capacityECCBaseValues[i + 13], capacityECCBaseValues[i + 14], capacityECCBaseValues[i + 15], capacityECCBaseValues[i + 16], capacityECCBaseValues[i + 17]),
                new ECCInfo((i + 24) / 24, ECCLevel.H, capacityECCBaseValues[i + 18], capacityECCBaseValues[i + 19], capacityECCBaseValues[i + 20], capacityECCBaseValues[i + 21], capacityECCBaseValues[i + 22], capacityECCBaseValues[i + 23])
            });
        }

        return list;
    }

    private static List<VersionInfo> CreateCapacityTable()
    {
        List<VersionInfo> list = new List<VersionInfo>(40);
        for (int i = 0; i < 640; i += 16)
        {
            list.Add(new VersionInfo((i + 16) / 16, new List<VersionInfoDetails>(4)
            {
                new VersionInfoDetails(ECCLevel.L, new Dictionary<EncodingMode, int>
                {
                    {
                        EncodingMode.Numeric,
                        capacityBaseValues[i]
                    },
                    {
                        EncodingMode.Alphanumeric,
                        capacityBaseValues[i + 1]
                    },
                    {
                        EncodingMode.Byte,
                        capacityBaseValues[i + 2]
                    },
                    {
                        EncodingMode.Kanji,
                        capacityBaseValues[i + 3]
                    }
                }),
                new VersionInfoDetails(ECCLevel.M, new Dictionary<EncodingMode, int>
                {
                    {
                        EncodingMode.Numeric,
                        capacityBaseValues[i + 4]
                    },
                    {
                        EncodingMode.Alphanumeric,
                        capacityBaseValues[i + 5]
                    },
                    {
                        EncodingMode.Byte,
                        capacityBaseValues[i + 6]
                    },
                    {
                        EncodingMode.Kanji,
                        capacityBaseValues[i + 7]
                    }
                }),
                new VersionInfoDetails(ECCLevel.Q, new Dictionary<EncodingMode, int>
                {
                    {
                        EncodingMode.Numeric,
                        capacityBaseValues[i + 8]
                    },
                    {
                        EncodingMode.Alphanumeric,
                        capacityBaseValues[i + 9]
                    },
                    {
                        EncodingMode.Byte,
                        capacityBaseValues[i + 10]
                    },
                    {
                        EncodingMode.Kanji,
                        capacityBaseValues[i + 11]
                    }
                }),
                new VersionInfoDetails(ECCLevel.H, new Dictionary<EncodingMode, int>
                {
                    {
                        EncodingMode.Numeric,
                        capacityBaseValues[i + 12]
                    },
                    {
                        EncodingMode.Alphanumeric,
                        capacityBaseValues[i + 13]
                    },
                    {
                        EncodingMode.Byte,
                        capacityBaseValues[i + 14]
                    },
                    {
                        EncodingMode.Kanji,
                        capacityBaseValues[i + 15]
                    }
                })
            }));
        }

        return list;
    }

    private static List<Antilog> CreateAntilogTable()
    {
        List<Antilog> list = new List<Antilog>(256);
        int num = 1;
        for (int i = 0; i < 256; i++)
        {
            list.Add(new Antilog(i, num));
            num *= 2;
            if (num > 255)
            {
                num ^= 0x11D;
            }
        }

        return list;
    }

    public void Dispose()
    {
    }
}


