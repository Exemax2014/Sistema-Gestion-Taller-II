using ClosedXML.Excel;

namespace Capa_Vistas
{
    internal sealed class ExcelHojaExportacion
    {
        public string Nombre { get; init; } = string.Empty;
        public IReadOnlyList<string> Encabezados { get; init; } = Array.Empty<string>();
        public IReadOnlyList<object?[]> Filas { get; init; } = Array.Empty<object?[]>();
        public bool CrearTabla { get; init; } = true;
        public IReadOnlyDictionary<int, string> FormatosColumnas { get; init; } = new Dictionary<int, string>();
        public IReadOnlyDictionary<(int Fila, int Columna), string> FormatosCeldas { get; init; } = new Dictionary<(int Fila, int Columna), string>();
    }

    internal static class ExcelExportHelper
    {
        // Genera un libro XLSX con valores tipados, tablas filtrables y formatos legibles.
        public static void Exportar(string ruta, IEnumerable<ExcelHojaExportacion> hojas)
        {
            using XLWorkbook libro = new();
            int numeroTabla = 1;

            foreach (ExcelHojaExportacion hoja in hojas)
            {
                IXLWorksheet pagina = libro.Worksheets.Add(hoja.Nombre);
                for (int columna = 0; columna < hoja.Encabezados.Count; columna++)
                {
                    IXLCell celda = pagina.Cell(1, columna + 1);
                    celda.Value = hoja.Encabezados[columna];
                    celda.Style.Font.Bold = true;
                }

                for (int fila = 0; fila < hoja.Filas.Count; fila++)
                {
                    for (int columna = 0; columna < hoja.Filas[fila].Length; columna++)
                        AsignarValor(pagina.Cell(fila + 2, columna + 1), hoja.Filas[fila][columna]);
                }

                foreach (((int fila, int columna), string formato) in hoja.FormatosCeldas)
                    pagina.Cell(fila + 2, columna + 1).Style.NumberFormat.Format = formato;

                if (hoja.CrearTabla && hoja.Filas.Count > 0 && hoja.Encabezados.Count > 0)
                {
                    IXLTable tabla = pagina.Range(1, 1, hoja.Filas.Count + 1, hoja.Encabezados.Count)
                        .CreateTable($"TablaReporte{numeroTabla++}");
                    tabla.Theme = XLTableTheme.TableStyleMedium2;
                }
                if (hoja.Encabezados.Count > 0) pagina.SheetView.FreezeRows(1);

                foreach ((int indice, string formato) in hoja.FormatosColumnas)
                    pagina.Column(indice + 1).Style.NumberFormat.Format = formato;

                for (int columna = 1; columna <= hoja.Encabezados.Count; columna++)
                {
                    pagina.Column(columna).AdjustToContents(1, Math.Max(1, hoja.Filas.Count + 1));
                    pagina.Column(columna).Width = Math.Clamp(pagina.Column(columna).Width, 10, 55);
                }
            }

            libro.SaveAs(ruta);
        }

        // Conserva números y fechas como celdas nativas en lugar de convertirlos a texto.
        private static void AsignarValor(IXLCell celda, object? valor)
        {
            switch (valor)
            {
                case null: celda.Clear(XLClearOptions.Contents); break;
                case string texto: celda.Value = texto; break;
                case DateTime fecha: celda.Value = fecha; break;
                case decimal decimalValor: celda.Value = decimalValor; break;
                case double doble: celda.Value = doble; break;
                case float simple: celda.Value = simple; break;
                case int entero: celda.Value = entero; break;
                case long enteroLargo: celda.Value = enteroLargo; break;
                case short enteroCorto: celda.Value = enteroCorto; break;
                case bool booleano: celda.Value = booleano; break;
                default: celda.Value = valor.ToString() ?? string.Empty; break;
            }
        }
    }
}
