using System.Globalization;
using Capa_Logica;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Capa_Vistas
{
    // ============================================================
    // ComprobanteVentaPdf
    //
    // Genera el comprobante PDF de una venta a partir de un
    // VentaDetalleModelo ya cargado. No accede a la base de datos.
    // Reutilizable desde cualquier origen del detalle de venta.
    // ============================================================

    internal static class ComprobanteVentaPdf
    {
        private static readonly CultureInfo Cultura =
            new CultureInfo("es-AR");

        private const double Margen = 40;
        private const double AltoFila = 18;

        public static string NombreSugerido(VentaDetalleModelo venta)
        {
            return $"Comprobante_Venta_{venta.IdVenta}.pdf";
        }

        public static void Generar(string ruta, VentaDetalleModelo venta)
        {
            using PdfDocument documento = new PdfDocument();
            documento.Info.Title = $"Comprobante de venta N° {venta.IdVenta}";
            documento.Info.Creator = "Sistema Hierro y Forja";

            XFont fTitulo = new XFont("Arial", 20, XFontStyleEx.Bold);
            XFont fSub = new XFont("Arial", 11, XFontStyleEx.Regular);
            XFont fNormal = new XFont("Arial", 10, XFontStyleEx.Regular);
            XFont fNegrita = new XFont("Arial", 10, XFontStyleEx.Bold);
            XFont fTotal = new XFont("Arial", 13, XFontStyleEx.Bold);

            XBrush negro = XBrushes.Black;
            XBrush gris = new XSolidBrush(XColor.FromArgb(90, 90, 90));
            XBrush encabezadoFondo = new XSolidBrush(XColor.FromArgb(45, 45, 45));
            XPen linea = new XPen(XColor.FromArgb(190, 190, 190), 0.6);

            PdfPage pagina = documento.AddPage();
            pagina.Size = PdfSharp.PageSize.A4;
            XGraphics g = XGraphics.FromPdfPage(pagina);

            double ancho = pagina.Width.Point;
            double alto = pagina.Height.Point;
            double anchoUtil = ancho - Margen * 2;
            double y = Margen;

            // Columnas de productos
            double xCodigo = Margen + 4;
            double xProducto = Margen + 90;
            double xCant = Margen + anchoUtil - 195;
            double xPrecio = Margen + anchoUtil - 135;
            double xSubtotal = Margen + anchoUtil - 4;

            void NuevaPagina()
            {
                g.Dispose();
                pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                g = XGraphics.FromPdfPage(pagina);
                y = Margen;
            }

            void DibujarEncabezadoTabla()
            {
                g.DrawRectangle(encabezadoFondo, Margen, y, anchoUtil, AltoFila);
                g.DrawString("Código", fNegrita, XBrushes.White, xCodigo, y + 13);
                g.DrawString("Producto", fNegrita, XBrushes.White, xProducto, y + 13);
                DibujarDerecha("Cant.", fNegrita, XBrushes.White, xCant + 40, y + 13);
                DibujarDerecha("Precio", fNegrita, XBrushes.White, xPrecio + 55, y + 13);
                DibujarDerecha("Subtotal", fNegrita, XBrushes.White, xSubtotal, y + 13);
                y += AltoFila;
            }

            void DibujarDerecha(string texto, XFont fuente, XBrush pincel, double xDerecha, double yBase)
            {
                double w = g.MeasureString(texto, fuente).Width;
                g.DrawString(texto, fuente, pincel, xDerecha - w, yBase);
            }

            string Recortar(string texto, XFont fuente, double maximo)
            {
                if (g.MeasureString(texto, fuente).Width <= maximo)
                    return texto;

                while (texto.Length > 1
                    && g.MeasureString(texto + "…", fuente).Width > maximo)
                    texto = texto[..^1];

                return texto + "…";
            }

            // ----- Cabecera -----
            g.DrawString("Hierro y Forja", fTitulo, negro, Margen, y + 18);
            string titulo = $"Comprobante de venta N° {venta.IdVenta}";
            DibujarDerecha(titulo, fSub, negro, Margen + anchoUtil, y + 14);
            y += 34;
            g.DrawLine(linea, Margen, y, Margen + anchoUtil, y);
            y += 16;

            // ----- Datos de la venta -----
            void Dato(string etiqueta, string valor, double x, double yy, double maxAncho)
            {
                g.DrawString(etiqueta, fNegrita, negro, x, yy);
                double wEt = g.MeasureString(etiqueta + " ", fNegrita).Width;
                g.DrawString(
                    Recortar(string.IsNullOrWhiteSpace(valor) ? "-" : valor, fNormal, maxAncho - wEt),
                    fNormal, gris, x + wEt, yy);
            }

            double mitad = anchoUtil / 2;
            Dato("Fecha y hora:", venta.FechaHora.ToString("dd/MM/yyyy HH:mm", Cultura), Margen, y, mitad - 10);
            Dato("Sucursal:", venta.Sucursal, Margen + mitad, y, mitad);
            y += 16;
            Dato("Cliente:", venta.Cliente, Margen, y, mitad - 10);
            Dato("Documento:", venta.DocumentoCliente, Margen + mitad, y, mitad);
            y += 16;
            Dato("Vendedor:", venta.Vendedor, Margen, y, mitad - 10);
            if (!string.IsNullOrWhiteSpace(venta.TipoFactura))
                Dato("Tipo de factura:", venta.TipoFactura, Margen + mitad, y, mitad);
            y += 28;

            // ----- Productos -----
            DibujarEncabezadoTabla();

            foreach (VentaItemModelo item in venta.Items)
            {
                if (y + AltoFila > alto - Margen - 120)
                {
                    NuevaPagina();
                    DibujarEncabezadoTabla();
                }

                g.DrawString(Recortar(item.CodigoBarra ?? string.Empty, fNormal, 80), fNormal, negro, xCodigo, y + 13);
                g.DrawString(Recortar(item.Producto, fNormal, xCant - xProducto - 10), fNormal, negro, xProducto, y + 13);
                DibujarDerecha(item.Cantidad.ToString(Cultura), fNormal, negro, xCant + 40, y + 13);
                DibujarDerecha(item.PrecioUnitario.ToString("C2", Cultura), fNormal, negro, xPrecio + 55, y + 13);
                DibujarDerecha(item.Subtotal.ToString("C2", Cultura), fNormal, negro, xSubtotal, y + 13);
                y += AltoFila;
                g.DrawLine(linea, Margen, y, Margen + anchoUtil, y);
            }

            y += 18;

            // ----- Pagos y totales -----
            double alturaBloque = 24 + venta.Pagos.Count * 16 + 90;
            if (y + alturaBloque > alto - Margen)
                NuevaPagina();

            g.DrawString("Métodos de pago", fNegrita, negro, Margen, y + 10);
            y += 18;
            foreach (VentaPagoModelo pago in venta.Pagos)
            {
                g.DrawString(Recortar(pago.MetodoPago, fNormal, 220), fNormal, negro, Margen + 6, y + 10);
                DibujarDerecha(pago.Monto.ToString("C2", Cultura), fNormal, negro, Margen + 250, y + 10);
                y += 16;
            }

            double xEtiquetaTot = Margen + anchoUtil - 200;
            // Totales a la derecha, alineados con el bloque de pagos
            double yT = y - (venta.Pagos.Count * 16) - 8;
            g.DrawString("Subtotal:", fNormal, negro, xEtiquetaTot, yT + 10);
            DibujarDerecha(venta.Subtotal.ToString("C2", Cultura), fNormal, negro, Margen + anchoUtil, yT + 10);
            g.DrawString("Descuento:", fNormal, negro, xEtiquetaTot, yT + 28);
            DibujarDerecha(venta.Descuento.ToString("C2", Cultura), fNormal, negro, Margen + anchoUtil, yT + 28);
            g.DrawLine(linea, xEtiquetaTot, yT + 36, Margen + anchoUtil, yT + 36);
            g.DrawString("TOTAL:", fTotal, negro, xEtiquetaTot, yT + 54);
            DibujarDerecha(venta.Total.ToString("C2", Cultura), fTotal, negro, Margen + anchoUtil, yT + 54);

            y = Math.Max(y, yT + 70) + 20;

            g.DrawString(
                $"Emitido el {DateTime.Now.ToString("dd/MM/yyyy HH:mm", Cultura)}",
                new XFont("Arial", 8, XFontStyleEx.Regular),
                gris,
                Margen,
                alto - Margen + 10);

            g.Dispose();
            documento.Save(ruta);
        }
    }
}
