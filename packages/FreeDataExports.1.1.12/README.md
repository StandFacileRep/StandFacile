# Free Data Exports (.NET)

Author: Ryan Kueter  
Updated: December, 2024

## About

**Free Data Exports** is a free .NET library, available from the [NuGet Package Manager](https://www.nuget.org/packages/FreeDataExports), that provides a simple and quick way to export data to a spreadsheet.  

##### Supported file types:
- Office Open XML Spreadsheet (**.xlsx**) v2019
- Open Document File (ODF) Spreadsheet (**.ods**) v1.3
- Comma-separated Values (**.csv**)

##### Version Support:
- .NET 6 - .NET 9
- .NET Standard 2.0
- Core 2 - 3.1
- Framework 4.6.1 - 4.8
- Mono 5.4, 6.4
- Xamarin.iOS 10.14, 12.16
- Xamarin.Android 8.0, 10.0
- Universal Windows Platform 10.0.16299
- Unity 2018.1
- https://dotnet.microsoft.com/platform/dotnet-standard#versions  
   
## Initialization

Free Data Exports provides an interface that allows the developer to reduce code duplication when creating a spreadsheet in both formats. However, be aware that the values for tab color or column width are different when creating an .xlsx file or an .ods file. 
```csharp
// Namespace
using FreeDataExports;

// Create a new workbook
IDataWorkbook workbook = new DataExport().CreateXLSX2019();
IDataWorkbook workbook = new DataExport().CreateODSv1_3();
```

## How to Create an XLSX (2019) File

An example of how to create a new .xlsx workbook in C#:

```csharp
// Namespace
using FreeDataExports;

// Create a new workbook
var workbook = new DataExport().CreateXLSX2019();

// Optional - Add some metadata
workbook.CreatedBy = "Jane Doe";

// Optional - Change the font size
workbook.FontSize = 11;

// Create worksheets
var orders = workbook.AddWorksheet("Orders");
var inventory = workbook.AddWorksheet("Inventory");

// Add column titles
orders.AddRow()
	.AddCell("OrderId", DataType.String)
	.AddCell("Item", DataType.String)
	.AddCell("Units", DataType.String)
	.AddCell("Price", DataType.String)
	.AddCell("OrderDate", DataType.String)
	.AddCell("SalesAssoc", DataType.String)
	.AddCell("Delivered", DataType.String);

// Add data
foreach (var o in Orders)
{
	orders.AddRow()
		.AddCell(o.OrderId, DataType.Number)
		.AddCell(o.Item, DataType.String)
		.AddCell(o.Units, DataType.Number)
		.AddCell(o.Price, DataType.Currency)
		.AddCell(o.OrderDate, DataType.LongDate)
		.AddCell(o.SalesAssociate, DataType.String)
		.AddCell(o.Delivered, DataType.Boolean);
}

// Add column titles
inventory.AddRow()
	.AddCell("ItemId", DataType.String)
	.AddCell("Item", DataType.String)
	.AddCell("Number", DataType.String)
	.AddCell("Price", DataType.String);

// Add data
foreach (var i in Inventory)
{
	inventory.AddRow()
		.AddCell(i.ItemId, DataType.Number)
		.AddCell(i.Item, DataType.String)
		.AddCell(i.Number, DataType.Number)
		.AddCell(i.Price, DataType.Currency);
}

// Optional - Add a tab color in RGB
orders.TabColor = "0000FF00"; // Green
inventory.TabColor = "000000FF"; // Blue

// Optional - Add column widths, based on character widths
orders.ColumnWidths("10", "10", "5", "10", "28.5", "15", "10");
inventory.ColumnWidths("10", "10", "10", "10");

// Optional - Reformat a data type
workbook.Format(DataType.DateTime24, @"m/d/yy\ h:mm;@");

// Optional - Add a worksheet to display data type conversion errors, only if they occur
workbook.AddErrorsWorksheet();

// Optional - Get the error manually
workbook.GetErrors();

// Synchronous GetBytes method
workbook.GetBytes();

// Asynchronous GetBytes method
await workbook.GetBytesAsync();

// Synchronous save method
workbook.Save(path);

// Asynchronous save method
await workbook.SaveAsync(path);
```  
   

## How to Create an ODS (1.3) File

The following code block is an example of how to create a new .ods workbook in C#. 

```csharp
// Namespaces
using FreeDataExports;

// Create a new workbook
var workbook = new DataExport().CreateODSv1_3();

// Optional - Add some metadata
workbook.CreatedBy = "Jane Doe";

// Optional - Change the font size
workbook.FontSize = 11;

// Create worksheets
var orders = workbook.AddWorksheet("Orders");
var inventory = workbook.AddWorksheet("Inventory");

// Add column titles
orders.AddRow()
	.AddCell("OrderId", DataType.String)
	.AddCell("Item", DataType.String)
	.AddCell("Units", DataType.String)
	.AddCell("Price", DataType.String)
	.AddCell("OrderDate", DataType.String)
	.AddCell("SalesAssoc", DataType.String)
	.AddCell("Delivered", DataType.String);

// Add data
foreach (var o in Orders)
{
	orders.AddRow()
		.AddCell(o.OrderId, DataType.Number)
		.AddCell(o.Item, DataType.String)
		.AddCell(o.Units, DataType.Number)
		.AddCell(o.Price, DataType.Currency)
		.AddCell(o.OrderDate, DataType.LongDate)
		.AddCell(o.SalesAssociate, DataType.String)
		.AddCell(o.Delivered, DataType.Boolean);
}

// Add column titles
inventory.AddRow()
	.AddCell("ItemId", DataType.String)
	.AddCell("Item", DataType.String)
	.AddCell("Number", DataType.String)
	.AddCell("Price", DataType.String);

// Add data
foreach (var i in Inventory)
{
	inventory.AddRow()
		.AddCell(i.ItemId, DataType.Number)
		.AddCell(i.Item, DataType.String)
		.AddCell(i.Number, DataType.Number)
		.AddCell(i.Price, DataType.Currency);
}

// Optional - Format the tab color in Hexadecimal
orders.TabColor = "#00FF00"; // Green
inventory.TabColor = "#0000FF"; // Blue

// Optional - Add column widths in inches or centimeters (Specify the unit of measure)
orders.ColumnWidths(".8in", "1in", ".5in", "1in", "1.5in", "1in", "1in");
inventory.ColumnWidths(".8in", "1in", ".8in", "1in");

// Optional - Reformat the datatypes
workbook.Format(DataType.Decimal, "decimals=8");
workbook.Format(DataType.Currency, "symbol=$,language=en,country=US,decimals=2");

// Optional - Add a worksheet to display data type conversion errors, only if they occur
workbook.AddErrorsWorksheet();

// Optional - Get the error manually
workbook.GetErrors();

// Synchronous GetBytes method
workbook.GetBytes();

// Asynchronous GetBytes method
await workbook.GetBytesAsync();

// Synchronous save method
workbook.Save(path);

// Asynchronous save method
await workbook.SaveAsync(path);
```  
   

## How to Create a CSV File

An example of how to create a new .csv file in C#:

```csharp
// Namespace
using FreeDataExports;

// Create a new csv file
var csv = new DataExport().CreateCsv();

// Add a header row
csv.AddRow("OrderId", "Item", "Units", "Price", "OrderDate", "SalesAssoc", "Delivered");

// Add some data
foreach (var o in Orders)
{
    csv.AddRow(o.OrderId, o.Item, o.Units, o.Price, o.OrderDate, o.SalesAssociate, o.Delivered);
}
            
// Synchronous GetBytes method
csv.GetBytes();

// Asynchronous GetBytes method
await csv.GetBytesAsync();

// Synchronous save method
csv.Save(path);

// Asynchronous save method
await csv.SaveAsync(path);
```  
   
## Formating Options

#### XLSX Formatting Strings

The following formatting strings are stored in the document and allow you to customize the format of numeric data types. Please note that many of the .xlsx formatting strings were written in C# and are string literals.

 - Currency: ```@"""$""#,##0.00"```
 - Percent: ```@"0.00%"```
 - DateTime: ```@"[$-409]m/d/yyyy\ h:mm:ss\ AM/PM;@"```
 - Decimal: ```"0.0000"```
 - Date: ```@"m/d/yyyy"```
 - Time: ```@"h:mm\ AM/PM"```
 - LongDate: ```@"[$-F800]dddd\,\ mmmm\ dd\,\ yyyy"```
 - DateTime24: ```@"m/d/yyyy\ h:mm:ss;@"```
 - Time24: ```@"h:mm:ss;@"```

#### ODS Formatting Strings

ODS files do not contain formatting strings like those above. So, the following formatting options are provided:

- Currency: ```"symbol=$,language=en,country=US,decimals=2"```
  - Alternatively: ```"decimals=2"```
- Decimal: ```"decimals=4"```
   

## Data Types

The library currently contains the following built in datatypes, which can be customized using the formating options:

-  String
-  Number
-  Currency
-  Percent
-  DateTime
-  Decimal
-  Date
-  Time
-  LongDate
-  DateTime24
-  Time24
-  Boolean