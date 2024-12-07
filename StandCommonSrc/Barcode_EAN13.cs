/*************************************************************************************
	NomeFile : StandCommonSrc/Barcode_EAN13.cs
	Data	 : 23.06.2023
	Autore   : Mauro Artuso
 *************************************************************************************/

using System;
using System.Windows.Forms;

namespace StandCommonFiles
{
    /// <summary>
    /// classe per la generazione del barcode EAN13<br/>
    /// buildBarcodeID() accetta in ingresso una stringa numerica di 12 caratteri<br/>
    /// a cui aggiunge il Ckecksum<br/>
    /// <br/>
    /// le stringhe per la stampa sono restituite in _sBuilString, _sBarcodeID<br/>
    /// sito utile https://barcode.tec-it.com/it
    /// </summary>
    public class Barcode_EAN13
    {
        static int[,] parityToEnc;
        static String[,] encTable;

        /// <summary>grafica barcode</summary>
        public static String _sBuilString;

        /// <summary>testo barcode</summary>
        public static String _sBarcodeID;

        /// <summary>
        /// costruttore
        /// </summary>
        public Barcode_EAN13()
        {
            parityToEnc = new int[10, 6];
            encTable = new String[3, 10];

            // sequenza delle codifiche

            parityToEnc[0, 0] = 0; // AABABB
            parityToEnc[0, 1] = 0;
            parityToEnc[0, 2] = 0;
            parityToEnc[0, 3] = 0;
            parityToEnc[0, 4] = 0;
            parityToEnc[0, 5] = 0;
            parityToEnc[1, 0] = 0; // AABABB
            parityToEnc[1, 1] = 0;
            parityToEnc[1, 2] = 1;
            parityToEnc[1, 3] = 0;
            parityToEnc[1, 4] = 1;
            parityToEnc[1, 5] = 1;
            parityToEnc[2, 0] = 0; // AABBAB
            parityToEnc[2, 1] = 0;
            parityToEnc[2, 2] = 1;
            parityToEnc[2, 3] = 1;
            parityToEnc[2, 4] = 0;
            parityToEnc[2, 5] = 1;
            parityToEnc[3, 0] = 0; // AABBBA
            parityToEnc[3, 1] = 0;
            parityToEnc[3, 2] = 1;
            parityToEnc[3, 3] = 1;
            parityToEnc[3, 4] = 1;
            parityToEnc[3, 5] = 0;
            parityToEnc[4, 0] = 0; // ABAABB
            parityToEnc[4, 1] = 1;
            parityToEnc[4, 2] = 0;
            parityToEnc[4, 3] = 0;
            parityToEnc[4, 4] = 1;
            parityToEnc[4, 5] = 1;
            parityToEnc[5, 0] = 0; // ABBAAB
            parityToEnc[5, 1] = 1;
            parityToEnc[5, 2] = 1;
            parityToEnc[5, 3] = 0;
            parityToEnc[5, 4] = 0;
            parityToEnc[5, 5] = 1;
            parityToEnc[6, 0] = 0; // ABBBAA
            parityToEnc[6, 1] = 1;
            parityToEnc[6, 2] = 1;
            parityToEnc[6, 3] = 1;
            parityToEnc[6, 4] = 0;
            parityToEnc[6, 5] = 0;
            parityToEnc[7, 0] = 0; // ABABAB
            parityToEnc[7, 1] = 1;
            parityToEnc[7, 2] = 0;
            parityToEnc[7, 3] = 1;
            parityToEnc[7, 4] = 0;
            parityToEnc[7, 5] = 1;
            parityToEnc[8, 0] = 0; // ABABBA
            parityToEnc[8, 1] = 1;
            parityToEnc[8, 2] = 0;
            parityToEnc[8, 3] = 1;
            parityToEnc[8, 4] = 1;
            parityToEnc[8, 5] = 0;
            parityToEnc[9, 0] = 0; // ABBABA
            parityToEnc[9, 1] = 1;
            parityToEnc[9, 2] = 1;
            parityToEnc[9, 3] = 0;
            parityToEnc[9, 4] = 1;
            parityToEnc[9, 5] = 0;

            // tabella delle codifiche A, B, C
            encTable[0, 0] = "0001101";
            encTable[0, 1] = "0011001";
            encTable[0, 2] = "0010011";
            encTable[0, 3] = "0111101";
            encTable[0, 4] = "0100011";
            encTable[0, 5] = "0110001";
            encTable[0, 6] = "0101111";
            encTable[0, 7] = "0111011";
            encTable[0, 8] = "0110111";
            encTable[0, 9] = "0001011";
            encTable[1, 0] = "0100111";
            encTable[1, 1] = "0110011";
            encTable[1, 2] = "0011011";
            encTable[1, 3] = "0100001";
            encTable[1, 4] = "0011101";
            encTable[1, 5] = "0111001";
            encTable[1, 6] = "0000101";
            encTable[1, 7] = "0010001";
            encTable[1, 8] = "0001001";
            encTable[1, 9] = "0010111";
            encTable[2, 0] = "1110010";
            encTable[2, 1] = "1100110";
            encTable[2, 2] = "1101100";
            encTable[2, 3] = "1000010";
            encTable[2, 4] = "1011100";
            encTable[2, 5] = "1001110";
            encTable[2, 6] = "1010000";
            encTable[2, 7] = "1000100";
            encTable[2, 8] = "1001000";
            encTable[2, 9] = "1110100";
        }

        /// <summary>
        /// verifica la correttezza del checkSum della stringa numerica lunga 13 caratteri <br/>
        /// considerando però solo i primi 13, se fallisce ritorna -1
        /// </summary>
        public static bool verifyChecksum(String sBarcodeParam)
        {
            // -100 per evitare che se anche la stringa è corta ritorna true
            int iCheckSum, iReadCheckSum = -100; 

            iCheckSum = buildChecksum(sBarcodeParam);

            if (sBarcodeParam.Length > 12)
                iReadCheckSum = Convert.ToInt32(sBarcodeParam[12]) - 48;

            return (iCheckSum == iReadCheckSum);
        }


        /// <summary>
        /// calcola il checksum sulla stringa numerica lunga almeno 12 caratteri <br/>
        /// considerando però solo i primi 12, se fallisce ritorna -1
        /// </summary>
        public static int buildChecksum(String sBarcodeParam)
        {
            int iCheckSum, iCheckSumCpl = -1;

            if (sBarcodeParam.Length > 12)
            {
                sBarcodeParam = sBarcodeParam.Substring(0, 12);
            }

            if (sBarcodeParam.Length == 12)
            {
                // calcolo del checksum :
                // si sommano le prime 12 cifre del codice alternativamente per 1 e per 3
                iCheckSum = Convert.ToInt32(sBarcodeParam[0]) - 48;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[1]) - 48) * 3;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[2]) - 48);
                iCheckSum += (Convert.ToInt32(sBarcodeParam[3]) - 48) * 3;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[4]) - 48);
                iCheckSum += (Convert.ToInt32(sBarcodeParam[5]) - 48) * 3;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[6]) - 48);
                iCheckSum += (Convert.ToInt32(sBarcodeParam[7]) - 48) * 3;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[8]) - 48);
                iCheckSum += (Convert.ToInt32(sBarcodeParam[9]) - 48) * 3;
                iCheckSum += (Convert.ToInt32(sBarcodeParam[10]) - 48);
                iCheckSum += (Convert.ToInt32(sBarcodeParam[11]) - 48) * 3;

                // il più piccolo numero da aggiungere a questa somma per ottenere un multiplo di 10
                // iCheckSum < 300 per cui lo si può sottrarre da 1000
                iCheckSumCpl = (1000 - iCheckSum) % 10;
            }
            else
                iCheckSumCpl = -1;

            return iCheckSumCpl;
        }

        /// <summary>stampa il barcode a partire da una stringa numerica lunga 12<br/>
        /// imposta _sBarcodeID, _sBuilString
        /// </summary>
        public static void buildBarcodeID(String sBarcode)
        {
            int i, iParityToEnc;
            int iFirstDigit, iCurDigit;

            String sFirstDigit, sCurDigit, sLastString;

            if (sBarcode.Length != 12)
            {
                MessageBox.Show("La lunghezza del Barcode deve essere di 12 cifre");
                return;
            }


            sBarcode += String.Format("{0}", buildChecksum(sBarcode)); // aggiunta del checksum

            sLastString = "101";
            sFirstDigit = sBarcode[0].ToString();
            iFirstDigit = Convert.ToInt32(sFirstDigit);

            for (i = 1; i <= 6; ++i)
            {
                sCurDigit = sBarcode[i].ToString();
                iCurDigit = Convert.ToInt32(sCurDigit);
                iParityToEnc = parityToEnc[iFirstDigit, i - 1];

                sLastString += encTable[iParityToEnc, iCurDigit];
            }

            sLastString += "01010";

            for (i = 7; i <= 12; ++i)
            {
                sCurDigit = sBarcode[i].ToString();
                iCurDigit = Convert.ToInt32(sCurDigit);
                sLastString += encTable[2, iCurDigit];
            }

            sLastString += "101";

            _sBarcodeID = sBarcode;
            _sBuilString = sLastString;
        }

    }
}