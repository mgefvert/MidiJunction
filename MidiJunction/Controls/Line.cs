using System;
using System.Drawing;
using System.Windows.Forms;

// ReSharper disable LocalizableElement

namespace MidiJunction.Controls
{
  public partial class Line : UserControl
  {
    public Line()
    {
      InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      var r = ClientRectangle;

      using (var pen = new Pen(ForeColor))
        e.Graphics.DrawLine(pen, r.X, r.Y, r.X + r.Width, r.Y);

      using (var pen = new Pen(Helper.Mix(ForeColor, Color.Black, 50)))
        e.Graphics.DrawLine(pen, r.X, r.Y + 2, r.X + r.Width, r.Y + 2);
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
      Invalidate();
    }

    private void OnLocationChanged(object sender, EventArgs e)
    {
      Invalidate();
    }
  }
}
