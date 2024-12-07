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
using static StandCommonFiles.commonCl;
using static StandCommonFiles.ReceiptAndCopies;
using StandFacile;
using System.IO;
using System.Collections.Generic;
using System;


namespace StandFacileTests
{

    [TestClass]
    public class ReceiptAndCopies_Tests
    {
        [TestMethod]
        public void CheckSomethingToPrint_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.Articolo[10].iQuantitaOrdine = 10;
            DB_Data.Articolo[10].iGruppoStampa = 2;// _bSomethingInto_GrpToPrint[2] = true;

            DB_Data.iGroupsColor[0] = 0;
            DB_Data.iGroupsColor[1] = 1; // _bSomethingInto_ClrToPrint[1] = true;
            DB_Data.iGroupsColor[2] = 1; // _bSomethingInto_ClrToPrint[2] = true;
            DB_Data.iGroupsColor[3] = 2;
            DB_Data.iGroupsColor[4] = 2;
            DB_Data.iGroupsColor[5] = 2;
            DB_Data.iGroupsColor[6] = 1; // _bSomethingInto_ClrToPrint[6] = true;
            DB_Data.iGroupsColor[7] = 3;

            // Act
            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data);

            bResult = _bSomethingInto_GrpToPrint[2];
            bResult &= _bSomethingInto_ClrToPrint[1] && _bSomethingInto_ClrToPrint[2] && _bSomethingInto_ClrToPrint[6];

            Assert.IsTrue(bResult);
        }
        [TestMethod]
        public void CheckSomethingToPrint_Test_IsFalse()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.Articolo[10].iQuantitaOrdine = 10;
            DB_Data.Articolo[10].iGruppoStampa = 2;// _bSomethingInto_GrpToPrint[2] = true;

            DB_Data.Articolo[10].bLocalPrinted = true; // per esito false

            DB_Data.iGroupsColor[0] = 0;
            DB_Data.iGroupsColor[1] = 1; // _bSomethingInto_ClrToPrint[1] = true;
            DB_Data.iGroupsColor[2] = 1; // _bSomethingInto_ClrToPrint[2] = true;
            DB_Data.iGroupsColor[3] = 2;
            DB_Data.iGroupsColor[4] = 2;
            DB_Data.iGroupsColor[5] = 2;
            DB_Data.iGroupsColor[6] = 1; // _bSomethingInto_ClrToPrint[6] = true;
            DB_Data.iGroupsColor[7] = 3;

            // Act
            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data, false);

            bResult = _bSomethingInto_GrpToPrint[2];
            bResult &= _bSomethingInto_ClrToPrint[1] && _bSomethingInto_ClrToPrint[2] && _bSomethingInto_ClrToPrint[6];

            Assert.IsFalse(bResult);
        }

        [TestMethod]
        public void CheckCopy_ToBePrintedOnce_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.iGroupsColor[0] = 0;
            DB_Data.iGroupsColor[1] = 1; // _bSomethingInto_ClrToPrint[1] = true;
            DB_Data.iGroupsColor[2] = 1; // _bSomethingInto_ClrToPrint[2] = true;
            DB_Data.iGroupsColor[3] = 2;
            DB_Data.iGroupsColor[4] = 2;
            DB_Data.iGroupsColor[5] = 2;
            DB_Data.iGroupsColor[6] = 1; // _bSomethingInto_ClrToPrint[6] = true;
            DB_Data.iGroupsColor[7] = 3;

            // mette true tutti gli elementi di _bSomethingInto_ClrToPrint[] che hanno iGroupsColor[k] == 1
            // Act
            bResult = CheckCopy_ToBePrintedOnce(1, _bSomethingInto_ClrToPrint, DB_Data);

            bResult &= _bSomethingInto_ClrToPrint[1] && _bSomethingInto_ClrToPrint[2] && _bSomethingInto_ClrToPrint[6];

            Assert.IsTrue(bResult);
        }

        [TestMethod]
        public void bCheckLastGroup_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.Articolo[10].iQuantitaOrdine = 10;
            DB_Data.Articolo[10].iGruppoStampa = 2;

            DB_Data.Articolo[12].iQuantitaOrdine = 10;
            DB_Data.Articolo[12].iGruppoStampa = 3;

            // Act
            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data, false);

            bResult = bCheckLastGroup(_bSomethingInto_GrpToPrint, 2);
            Assert.IsFalse(bResult);

            bResult = bCheckLastGroup(_bSomethingInto_GrpToPrint, 3);
            Assert.IsTrue(bResult);

            bResult = bCheckLastGroup(_bSomethingInto_GrpToPrint, 4);
            Assert.IsTrue(bResult);

        }

        [TestMethod]
        public void bCheckLastItemAndGroupToCut_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.Articolo[12].iQuantitaOrdine = 10;
            DB_Data.Articolo[12].iGruppoStampa = 3;

            // Act
            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data, false);

            bResult = bCheckLastItemAndGroupToCut(DB_Data, _bSomethingInto_GrpToPrint);
            Assert.IsFalse(bResult);

            DB_Data.Articolo[12].bLocalPrinted = true; // per esito true

            bResult = bCheckLastItemAndGroupToCut(DB_Data, _bSomethingInto_GrpToPrint);
            Assert.IsTrue(bResult);
        }

        [TestMethod]
        public void bCheckLastItemToCut_OnSameGroup_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            // Arrange
            DB_Data.Articolo[10].iQuantitaOrdine = 8;
            DB_Data.Articolo[10].iGruppoStampa = 3;

            DB_Data.Articolo[12].iQuantitaOrdine = 10;
            DB_Data.Articolo[12].iGruppoStampa = 3;

            // Act
            bResult = bCheckLastItemToCut_OnSameGroup(DB_Data, 3, 10);
            Assert.IsFalse(bResult);

            bResult = bCheckLastItemToCut_OnSameGroup(DB_Data, 3, 12);
            Assert.IsTrue(bResult);
        }

        [TestMethod]
        public void bTicketScontatoStdIsGood_Test_IsTrue()
        {
            bool bResult;

            TData DB_Data = new TData(0);
            bool[] bScontoGruppo = new bool[NUM_EDIT_GROUPS];

            // Arrange
            DB_Data.iStatusSconto = 0x000A0000;

            bScontoGruppo[1] = false;
            bScontoGruppo[2] = true;
            bScontoGruppo[3] = true;

            DB_Data.Articolo[10].iGruppoStampa = 2;
            DB_Data.Articolo[10].iQuantitaOrdine = 8;
            DB_Data.Articolo[10].iPrezzoUnitario = 240;

            DB_Data.Articolo[12].iGruppoStampa = 3;
            DB_Data.Articolo[12].iQuantitaOrdine = 10;
            DB_Data.Articolo[12].iPrezzoUnitario = 1200;

            // Act
            bResult = bTicketScontatoStdIsGood(DB_Data, bScontoGruppo);
            Assert.IsTrue(bResult);

            DB_Data.Articolo[10].iGruppoStampa = 1;
            DB_Data.Articolo[12].iGruppoStampa = 1;

            bResult = bTicketScontatoStdIsGood(DB_Data, bScontoGruppo);
            Assert.IsFalse(bResult);
        }
    }

}
