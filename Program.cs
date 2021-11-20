using System;
using System.Linq;
using System.Text;

namespace KNXParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter HEX String (3-Level Addressing):");
                    string inputHexLine;

                    inputHexLine = Console.ReadLine();

                    if (inputHexLine != null)
                    {
                        string binarystring = String.Join(String.Empty,
          inputHexLine.Select(
            c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
          )
        );
                        Console.WriteLine("Binary String is: \r\n" + binarystring);
                        for (int i = 0; i < binarystring.Length; i++)
                            Console.Write("=");
                        Console.WriteLine("");
                        // Control Flow 
                        Console.WriteLine("Control Flow (1Byte): " + binarystring.Substring(0, 8));
                        // Source Address
                        Console.Write("Source Address (2Byte): " + binarystring.Substring(8, 16));
                        Console.Write("      | Equals: " + Convert.ToInt32(binarystring.Substring(8, 4), 2) + ",");
                        Console.Write(Convert.ToInt32(binarystring.Substring(12, 4), 2) + ",");
                        Console.WriteLine(Convert.ToInt32(binarystring.Substring(16, 8), 2));
                        // Target Mode
                        Console.Write("Target Address (2Byte): " + binarystring.Substring(24, 16));
                        Console.Write("      | Equals: " + Convert.ToInt32(binarystring.Substring(24, 5), 2) + "/");
                        Console.Write(Convert.ToInt32(binarystring.Substring(29, 3), 2) + "/");
                        Console.WriteLine(Convert.ToInt32(binarystring.Substring(32, 8), 2));
                        // NDPU 
                        Console.WriteLine("NPDU (Network Protocol Data Units): ");
                        // Address Mode
                        if (Convert.ToInt32(binarystring.Substring(40, 1), 2) == 0)
                            Console.WriteLine("   Address Mode (1Bit): 0                     | Equals: Individual Address");
                        else if (Convert.ToInt32(binarystring.Substring(40, 1), 2) == 1)
                            Console.WriteLine("   Address Mode (1Bit): 1                     | Equals: Group Address");
                        // Routing Counter
                        Console.Write("   Routing Counter (3Bit): " + binarystring.Substring(41, 3));
                        Console.WriteLine("                | Equals: " + Convert.ToInt32(binarystring.Substring(41, 3), 2));
                        // Data Length
                        Console.Write("   Data Length (4Bit): " + binarystring.Substring(44, 4));
                        Console.WriteLine("                   | Equals: " + Convert.ToInt32(binarystring.Substring(44, 4), 2));
                        int dataByteLength = Convert.ToInt32(binarystring.Substring(44, 4), 2);
                        // TDPU 
                        Console.WriteLine("   TPDU (Transport Protocol Data Units)");
                        //TPCI (Transport Layer Protocol Control Information)
                        int TPCI = Convert.ToInt32(binarystring.Substring(48, 2), 2);
                        Console.Write("      TCPI (2Bit): " + binarystring.Substring(48, 2));
                        switch (TPCI)
                        {
                            case 0:
                                Console.WriteLine("                         | Equals: Unnumbered Data Packet (UDP)");
                                break;
                            case 1:
                                Console.WriteLine("                         | Equals: Numbered Data Packet (NDP) ");
                                break;
                            case 2:
                                Console.WriteLine("                         | Equals: Unnumbered Control Data (UCD)");

                                break;
                            case 3:
                                Console.WriteLine("                         | Equals: Numbered Control Data (NCD)");
                                break;
                        }
                        Console.Write("      Sequence Number (4Bit): " + binarystring.Substring(50, 4));
                        Console.WriteLine("            | Equals: " + Convert.ToInt32(binarystring.Substring(50, 4), 2));
                        // ADPU
                        if (dataByteLength != 0)
                        {
                            Console.WriteLine("      APDU (Application Protocol Data Units)");
                            int APCI = Convert.ToInt32(binarystring.Substring(54, 4), 2);
                            Console.Write("         ACPI (4Bit): " + binarystring.Substring(54, 4));
                            switch (APCI)
                            {
                                case 0:
                                    Console.WriteLine("                    | Equals: GroupValueRead");
                                    break;
                                case 1:
                                    Console.WriteLine("                    | Equals: GroupValueResponse");
                                    break;
                                case 2:
                                    Console.WriteLine("                    | Equals: GroupValueWrite");

                                    break;
                                case 3:
                                    Console.WriteLine("                    | Equals: IndividualAddrWrite");
                                    break;
                                case 4:
                                    Console.WriteLine("                    | Equals: IndividualAddrRequest");
                                    break;
                                case 5:
                                    Console.WriteLine("                    | Equals: IndividualAddrResponse");
                                    break;
                                case 6:
                                    Console.WriteLine("                    | Equals: AdcRead");
                                    break;
                                case 7:
                                    Console.WriteLine("                    | Equals: AdcResponse");
                                    break;
                                case 8:
                                    Console.WriteLine("                    | Equals: MemoryRead");
                                    break;
                                case 9:
                                    Console.WriteLine("                    | Equals: MemoryResponse");
                                    break;
                                case 10:
                                    Console.WriteLine("                    | Equals: MemoryWrite");
                                    break;
                                case 11:
                                    Console.WriteLine("                    | Equals: UserMessage");
                                    break;
                                case 12:
                                    Console.WriteLine("                    | Equals: MaskVersionRead");
                                    break;
                                case 13:
                                    Console.WriteLine("                    | Equals: MaskVersionResponse");
                                    break;
                                case 14:
                                    Console.WriteLine("                    | Equals: Restart");
                                    break;
                                case 15:
                                    Console.WriteLine("                    | Equals: Escape");
                                    break;
                            }
                        
                        // Data
                        if (dataByteLength == 1)
                        {
                            Console.Write("         Data (6Bit): " + binarystring.Substring(58, 6));
                            int Data = Convert.ToInt32(binarystring.Substring(58, 6), 2);
                            int DataPercentage = Data * 100 / 255;
                            string DataHex = Data.ToString("X");
                            Console.Write("                  | Equals: " + Data);
                            Console.Write(" - " + DataHex);
                            Console.WriteLine(" - " + DataPercentage + "%");
                        }
                        else
                            for (int i = 0; i < dataByteLength - 1; i++)
                            {
                                int Data = Convert.ToInt32(binarystring.Substring(i * 8 + 64, 8), 2);
                                int DataPercentage = Data * 100 / 255;
                                string DataHex = Data.ToString("X");
                                Console.Write("         Data Byte " + (i + 1) + ": " + binarystring.Substring(i * 8 + 64, 8));
                                Console.Write("                | Equals: " + Data);
                                Console.Write(" - " + DataHex);
                                Console.WriteLine(" - " + DataPercentage + "%");
                            }
                        }
                        // ACK Data
                        int intdataLength = binarystring.Length - 54;
                        Console.Write("         Data: " + binarystring.Substring(54, intdataLength));
                        Console.WriteLine("                             | Equals: " + Convert.ToInt32(binarystring.Substring(54, intdataLength), 2));

                        // Checksum
                        int checksumIndex = 64 + ((dataByteLength - 1) * 8);
                        int DataByteLength = binarystring.Length - 8;
                        // To Calculate Checksum first calc XOR for 8bit substrings and then do 1`s Compliment.
                        try
                        {
                            Console.Write("Checksum (1Byte): " + binarystring.Substring(checksumIndex, 8));
                            Console.Write("                    | Summery: ");
                            string xorOutput = null;
                            for (int i = 1; i * 8 < DataByteLength; i++)
                            {
                                if (i == 1)
                                    xorOutput = binarystring.Substring(0, 8);
                                xorOutput = xorIt(xorOutput, binarystring.Substring(i * 8, 8));
                                // Console.WriteLine("i: " + i);
                                // Console.WriteLine("xorOutput: " + xorOutput);
                            }
                            string BinaryNot(string xorOutput) => string.Concat(xorOutput.Select(x => x == '0' ? '1' : '0'));
                            string CalcedChecksum = BinaryNot(xorOutput);
                            if (String.Equals(binarystring.Substring(checksumIndex, 8), CalcedChecksum))
                            {
                                Console.WriteLine("Verification OK");
                            }
                            else
                            {
                                Console.WriteLine("Verification Failed");
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("Checksum                                      | Summery: No Checksum");
                        }
                        for (int i = 0; i < binarystring.Length; i++)
                            Console.Write("=");
                        Console.WriteLine("");
                    }

                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        public static string xorIt(string key, string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            string result = sb.ToString();
            return AsciiToHex(result);
        }

        public static string AsciiToHex(string asciiString)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in asciiString)
            {
                builder.Append(Convert.ToInt32(c).ToString("X"));
            }
            return builder.ToString();
        }
    }
}
// 10111100
// 00010001
// 00000010
// 00001011
// 00001110
// 11100001
// 00000000
// 10000000

// 00110100
// RAW Data: bc11020b0ee1008034
// Binary String: 
// CF (1Byte): 10111100
// SA (2Byte): 0001 0001 00000010 === 1.1.2
// TA (2Byte): 00001 011 00001110 === 1/3/14
// N_PDU:
//     AddrMode: 1 Group address
//     RC (3bit): 110 === NoRouting
//     Length: 0001 === 1
//     T_PDU:
//         TPCI: 00 === UDP
//         Sequence Number: 0000
//         A_PDU:
//             APCI: 0010 === GroupValueWrite
//             Data:  000000
// CheckByte: 00110100