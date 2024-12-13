/*****************************************************
 	NomeFile : StandFacileTests/StansFacileCommon_CLTests.cs
    Data	 : 20.10.2024
 	Autore	 : Mauro Artuso

	Classe per Unit Tests
 *****************************************************/

using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static StandFacile.Define;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ReceiptAndCopies;
using StandFacile;
using System.IO;
using System.Collections.Generic;
using System;


namespace StandFacileTests
{
    [TestClass]
    public class Common_CL_UnitTests
    {
#pragma warning disable IDE0017
#pragma warning disable IDE0059

        /***************************************************
            test methods conventions, there are 3 parts:

                1) name of method under test
                2) scenario
                3) extected behaviour

            inside each test method "Triple A":

                // Arrange
                    initialize Objects

                // Act
                    call metheods

                // Assert
                    evaluate with framework

            kind of test:
                unit
                components  happy path, unhappy paths
                integration test (components togheter)
                end-to-end (selenium with web browser)
                manual

        ***************************************************/

        [TestMethod]
        [DataRow(3, 20)]
        [DataRow(20, 30)]
        public void GetDateTimeString_ReturnsTrue(int iHourPrm, int iMinPrm)
        {
            DateTime expectedDateTime;
            string sDateTimeString, sExpectedDateTime;

            // Arrange
            InitActualDate(iHourPrm, iMinPrm);


            if (iHourPrm < 5)
                expectedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, iHourPrm, iMinPrm, 0);
            else
                expectedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, iHourPrm, iMinPrm, 0);

            sExpectedDateTime = expectedDateTime.ToString("ddd dd/MM/yy") + " " + expectedDateTime.ToString("HH.mm.ss");

            // Act
            sDateTimeString = GetDateTimeString(true);

            // Assert
            Assert.AreEqual(sDateTimeString, sExpectedDateTime);
        }

        [TestMethod]
        [DataRow("sconto bambini", 28, "       sconto bambini")]
        [DataRow("sconto bambini", 33, "          sconto bambini")]
        public void CenterJustify_ReturnsTrue(string SValParam, int iWidthParam, string sExpResultParam)
        {
            // Arrange

            // Act
            string sResult = CenterJustify(SValParam, iWidthParam);

            // Assert
            Assert.AreEqual(sResult, sExpResultParam);
        }

        [TestMethod]
        [DataRow("sconto bambini", 28, "  **   sconto bambini   **")]
        [DataRow("sconto bambini", 33, "  *****   sconto bambini   *****")]
        public void CenterJustifyStars_ReturnsTrue(string SValParam, int iWidthParam, string sExpResultParam)
        {
            // Arrange

            // Act
            string sResult = CenterJustifyStars(SValParam, iWidthParam, '*');

            // Assert
            Assert.AreEqual(sResult, sExpResultParam);
        }

        [TestMethod]
        [DataRow(125, "1,25")]
        [DataRow(1250, "12,50")]
        [DataRow(01250, "12,50")]
        [DataRow(80, "0,80")]
        [DataRow(8, "0,08")]
        public void IntToEuro_ReturnsTrue(int iValParam, string sExpResultParam)
        {
            // Arrange

            // Act
            string sResult = IntToEuro(iValParam);

            // Assert
            Assert.AreEqual(sResult, sExpResultParam);
        }

        [TestMethod]
        public void AddTo_ComboList_ReturnsTrue()
        {
            ComboBox comboBox = new ComboBox();

            comboBox.Text = "stringEntry_1";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            comboBox.Text = "stringEntry_2";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            comboBox.Text = "stringEntry_1";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            Assert.IsTrue(comboBox.Items.Count == 2);

            comboBox.Text = "stringEntry_3";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            comboBox.Text = "stringEntry_4";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            comboBox.Text = "stringEntry_5";
            AddTo_ComboList(comboBox, SEL_DB_SERVER_KEY);

            Assert.IsTrue(comboBox.Items.Count == MAX_COMBO_ITEMS);
            Assert.AreEqual(comboBox.Items[2], "stringEntry_3");
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow("0", 0)]
        [DataRow("10", 1000)]
        [DataRow("10.20", 1020)]
        [DataRow("-10.20", -1)]
        [DataRow("10 .20", -1)]
        [DataRow("10.200", -1)]
        [DataRow("10..20", -1)]
        public void EuroToInt_ReturnsTrue(string sValParam, int iExpResultParam)
        {
            // Arrange
            TErrMsg WrnMsg = new TErrMsg();

            // Act
            int iresult = EuroToInt(sValParam, EURO_CONVERSION.EUROCONV_DONT_CARE, WrnMsg);

            // Assert
            Assert.AreEqual(iresult, iExpResultParam);
        }

        [TestMethod]
        [DataRow("", -1)]
        [DataRow("0", -1)]
        public void EuroToInt_NZ_ReturnsTrue(string sValParam, int iExpResultParam)
        {
            // Arrange
            TErrMsg WrnMsg = new TErrMsg();

            // Act
            int iresult = EuroToInt(sValParam, EURO_CONVERSION.EUROCONV_NZ, WrnMsg);

            // Assert
            Assert.AreEqual(iresult, iExpResultParam);
        }

        [TestMethod]
        public void Arrotonda_ReturnsTrue()
        {
            // Arrange
            int iVal = 1008;
            int iExpected = 1010;

            // Act
            int iresult = Arrotonda(iVal);

            // Assert
            Assert.AreEqual(iExpected, iresult);
        }

        [TestMethod]
        public void Encrypt_Dencrypt_ReturnsTrue()
        {
            // Arrange
            string sStringToEncript = "sempre caro mi fu' quest'ermo colle";

            // Act
            string sEncripted = Encrypt(sStringToEncript);
            string sDecripted = Decrypt(sEncripted);

            // Assert
            Assert.AreEqual(sStringToEncript, sDecripted);
        }

        [TestMethod]
        public void DeepCopy_ReturnsTrue()
        {
            // Arrange
            TSconto ScontoOri = new TSconto(0);
            TSconto scontoCopy = new TSconto(0);

            ScontoOri.iStatusSconto = 0x11223344;
            ScontoOri.iScontoValPerc = 10;
            ScontoOri.iScontoValFisso = 2200;

            ScontoOri.bScontoGruppo[0] = true;
            ScontoOri.bScontoGruppo[1] = true;
            ScontoOri.bScontoGruppo[2] = true;
            ScontoOri.bScontoGruppo[3] = false;
            ScontoOri.bScontoGruppo[4] = true;
            ScontoOri.bScontoGruppo[5] = false;
            ScontoOri.bScontoGruppo[6] = true;
            ScontoOri.bScontoGruppo[7] = false;

            ScontoOri.sScontoText[(int)DISC_TYPE.DISC_NONE] = "sconto none";
            ScontoOri.sScontoText[(int)DISC_TYPE.DISC_STD] = "sconto standard";
            ScontoOri.sScontoText[(int)DISC_TYPE.DISC_FIXED] = "sconto fisso";
            ScontoOri.sScontoText[(int)DISC_TYPE.DISC_GRATIS] = "sconto gratis";

            // Act
            //DeepCopy2(ref ScontoOri, ref scontoCopy);
            scontoCopy = DeepCopy(ScontoOri);

            scontoCopy.iStatusSconto = ScontoOri.iStatusSconto + 4;
            scontoCopy.bScontoGruppo[7] = true;
            scontoCopy.sScontoText[(int)DISC_TYPE.DISC_FIXED] = "sconto fissssso";
            scontoCopy.sScontoText[(int)DISC_TYPE.DISC_GRATIS] = "sconto gratisssss";

            // Assert
            Assert.AreNotEqual(ScontoOri, scontoCopy);
        }

        [TestMethod]
        public void StringBelongsTo_ORDER_CONST_SCONTO_exc_ReturnsTrue()
        {
            // Arrange
            string sSconto = ORDER_CONST._START_OF_ORDER;

            // Act
            bool bResult = StringBelongsTo_ORDER_CONST(sSconto, ORDER_CONST._SCONTO);

            // Assert
            Assert.IsTrue(bResult);
        }

        [TestMethod]
        public void StringBelongsTo_ORDER_CONST_SCONTO_exc_ReturnsFalse()
        {
            // Arrange
            string sSconto = ORDER_CONST._SCONTO;

            // Act
            bool bResult = StringBelongsTo_ORDER_CONST(sSconto, ORDER_CONST._SCONTO);

            // Assert
            Assert.IsFalse(bResult);
        }

        [TestMethod]
        public void StringBelongsTo_ORDER_CONST_SCONTO_ReturnsTrue()
        {
            // Arrange
            string sSconto = ORDER_CONST._SCONTO;

            // Act
            bool bResult = StringBelongsTo_ORDER_CONST(sSconto);

            // Assert
            Assert.IsTrue(bResult);
        }

        [TestMethod]
        public void SetBit_08_3_Returns_88()
        {
            // Arrange
            int inputVar = 0x80;
            int iBitPos = 3;

            // Act
            int iResult = SetBit(inputVar, iBitPos);

            // Assert
            Assert.AreEqual(0x88, iResult);
        }

        [TestMethod]
        public void ClearBit_22_5_Returns_02()
        {
            // Arrange
            int inputVar = 0x22;
            int iBitPos = 5;

            // Act
            int iResult = ClearBit(inputVar, iBitPos);

            // Assert
            Assert.AreEqual(0x02, iResult);
        }

        [TestMethod]
        public void IsBitSet_24_2_ReturnsTrue()
        {
            // Arrange
            int inputVar = 0x24;
            int iBitPos = 2;

            // Act
            bool bResult = IsBitSet(inputVar, iBitPos);

            // Assert
            Assert.IsTrue(bResult);
        }
    }
}
