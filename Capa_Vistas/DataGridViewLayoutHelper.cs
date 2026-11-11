namespace Capa_Vistas
{
    // Centraliza restricciones visuales y mínimos de encabezado sin alterar bindings, eventos ni ordenamiento.
    internal static class DataGridViewLayoutHelper
    {
        public static void Normalizar(DataGridView grilla, params string[] columnasFill)
        {
            grilla.AllowUserToResizeColumns = false;
            grilla.AllowUserToResizeRows = false;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grilla.ScrollBars = ScrollBars.Both;
            grilla.RowTemplate.Height = Math.Max(34, grilla.RowTemplate.Height);

            HashSet<string> fill = columnasFill.ToHashSet(StringComparer.Ordinal);
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (!columna.Visible) continue;

                int textoCabecera = TextRenderer.MeasureText(
                    columna.HeaderText ?? string.Empty,
                    grilla.ColumnHeadersDefaultCellStyle.Font ?? grilla.Font).Width + 28;
                int minimo = Math.Max(64, textoCabecera);
                if (columna is DataGridViewButtonColumn boton && !string.IsNullOrWhiteSpace(boton.Text))
                    minimo = Math.Max(minimo, TextRenderer.MeasureText(boton.Text, grilla.Font).Width + 28);

                if (fill.Contains(columna.Name))
                {
                    columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columna.MinimumWidth = Math.Max(columna.MinimumWidth, minimo);
                }
                else
                {
                    columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    columna.Width = Math.Max(columna.Width, minimo);
                    columna.MinimumWidth = Math.Max(columna.MinimumWidth, minimo);
                }
            }
        }
    }
}
