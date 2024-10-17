using System;

public abstract class ReportGenerator
{
    // Шаблонный метод
    public void GenerateReport()
    {
        CollectData();
        FormatData();
        if (CustomerWantsSave())
        {
            SaveReport();
        }
        else
        {
            SendByEmail();
        }
    }

    // Шаги, которые могут быть переопределены
    protected abstract void CollectData();
    protected abstract void FormatData();
    protected abstract void SaveReport();
    protected virtual bool CustomerWantsSave() => true;

    // Опциональный шаг - можно переопределить или оставить дефолтным
    protected virtual void SendByEmail()
    {
        Console.WriteLine("Отправка отчета по электронной почте...");
    }
}

public class PdfReport : ReportGenerator
{
    protected override void CollectData() => Console.WriteLine("Сбор данных для PDF-отчета...");
    protected override void FormatData() => Console.WriteLine("Форматирование данных для PDF-отчета...");
    protected override void SaveReport() => Console.WriteLine("Сохранение PDF-отчета...");
}

public class ExcelReport : ReportGenerator
{
    protected override void CollectData() => Console.WriteLine("Сбор данных для Excel-отчета...");
    protected override void FormatData() => Console.WriteLine("Форматирование данных для Excel-отчета...");
    protected override void SaveReport() => Console.WriteLine("Сохранение Excel-отчета...");
    
    protected override bool CustomerWantsSave()
    {
        Console.Write("Хотите сохранить отчет? (y/n): ");
        string input = Console.ReadLine();
        return input.ToLower() == "y";
    }
}

public class HtmlReport : ReportGenerator
{
    protected override void CollectData() => Console.WriteLine("Сбор данных для HTML-отчета...");
    protected override void FormatData() => Console.WriteLine("Форматирование данных для HTML-отчета...");
    protected override void SaveReport() => Console.WriteLine("Сохранение HTML-отчета...");
}

public class CsvReport : ReportGenerator
{
    protected override void CollectData() => Console.WriteLine("Сбор данных для CSV-отчета...");
    protected override void FormatData() => Console.WriteLine("Форматирование данных для CSV-отчета...");
    protected override void SaveReport() => Console.WriteLine("Сохранение CSV-отчета...");
}

class Program
{
    static void Main()
    {
        ReportGenerator pdfReport = new PdfReport();
        ReportGenerator excelReport = new ExcelReport();
        ReportGenerator htmlReport = new HtmlReport();
        ReportGenerator csvReport = new CsvReport();

        Console.WriteLine("\nГенерация PDF отчета:");
        pdfReport.GenerateReport();

        Console.WriteLine("\nГенерация Excel отчета:");
        excelReport.GenerateReport();

        Console.WriteLine("\nГенерация HTML отчета:");
        htmlReport.GenerateReport();

        Console.WriteLine("\nГенерация CSV отчета:");
        csvReport.GenerateReport();
    }
}
