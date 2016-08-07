using System;
using System.Drawing;
using System.Windows.Forms;

// ReSharper disable LocalizableElement

namespace MidiJunction.Controls
{
  public partial class MidiBus : UserControl
  {
    private const string SmallFontName = "MS Reference Sans Serif";
    private static readonly Color ActiveChannelColor = Helper.Mix(Color.DodgerBlue, Color.FromArgb(51, 51, 51), 50);

    private int? _activeChannel;
    private readonly byte[] _opacity = new byte[16];
    private string _title;
    private Color _titleColor;
    private bool _vertical;

    public int? ActiveChannel
    {
      get { return _activeChannel; }
      set
      {
        if (_activeChannel == value)
          return;

        _activeChannel = value;
        Invalidate();
      }
    }

    public string Title
    {
      get { return _title; }
      set
      {
        if (_title == value)
          return;

        _title = value;
        Invalidate();
      }
    }

    public Color TitleColor
    {
      get { return _titleColor; }
      set
      {
        if (_titleColor == value)
          return;

        _titleColor = value;
        Invalidate();
      }
    }

    public bool Vertical
    {
      get { return _vertical; }
      set
      {
        if (_vertical == value)
          return;

        _vertical = value;
        Invalidate();
      }
    }

    public MidiBus()
    {
      InitializeComponent();
    }

    public void Tick()
    {
      var any = false;
      for (var i = 0; i < _opacity.Length; i++)
      {
        if (_opacity[i] == 0)
          continue;

        _opacity[i] = (byte)Math.Max(0, _opacity[i] - 8);
        any = true;
      }

      if (any)
        Invalidate();
    }

    public void TriggerChannel(int channel)
    {
      AssertChannel(channel);
      _opacity[channel] = 100;

      Invalidate();
    }

    private void AssertChannel(int channel)
    {
      if (channel < 0 || channel > 15)
        throw new ArgumentOutOfRangeException(nameof(channel), "Channel is out of range");
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      var r = ClientRectangle;
      var h2 = r.Height / 2;
      var w2 = r.Width / 2;

      var rtitle = (_vertical ? new Rectangle(r.X, r.Y, r.Width, h2).Modify(0, 0, -1, -3) : new Rectangle(r.X, r.Y, w2, r.Height).Modify(0, 0, -3, -1));
      var rboxes = _vertical ? new RectangleF(r.X, r.Y + h2, r.Width, r.Height - h2) : new RectangleF(r.X + w2, r.Y, r.Width - w2, r.Height);

      using (var brushBackground = new SolidBrush(BackColor))
      using (var brushForeground = new SolidBrush(ForeColor))
      using (var brushTitleColor = new SolidBrush(TitleColor))
      {
        var stringFormat = new StringFormat(StringFormatFlags.NoWrap)
        {
          Alignment = StringAlignment.Center,
          LineAlignment = StringAlignment.Center
        };

        var g = e.Graphics;
        {
          // Draw background
          if (BackColor.A != 0)
            g.FillRectangle(brushBackground, r);

          // Draw title
          if (TitleColor.A == 0)
          {
            g.DrawRectangle(new Pen(Color.White.SetAlpha(0x40)), rtitle);
            g.DrawRectangle(new Pen(Color.White.SetAlpha(0x20)), rtitle.Modify(2, 2, -4, -4));
          }
          else
            g.FillRectangle(brushTitleColor, rtitle);

          g.DrawString(Title, Font, brushForeground, rtitle, stringFormat);

          // Draw 16 little boxes
          var wbox = rboxes.Width/8;
          var hbox = rboxes.Height/2;

          // Figure out best font size
          int bestSize;
          for (bestSize = 3; bestSize < 20; bestSize++)
          {
            using (var fontTest = new Font(SmallFontName, bestSize, FontStyle.Regular, GraphicsUnit.Pixel))
            {
              var sz = g.MeasureString("13", fontTest);
              if (sz.Height >= hbox || sz.Width >= wbox)
                break;
            }
          }

          // Draw
          using (var fontMini = new Font(SmallFontName, bestSize - 1, FontStyle.Regular, GraphicsUnit.Pixel))
          {
            var channel = 0;
            for (var y = 0; y < 2; y++)
              for (var x = 0; x < 8; x++)
              {
                var box = new RectangleF(rboxes.X + wbox*x, rboxes.Y + hbox*y, wbox, hbox);

                if (_opacity[channel] != 0 || _activeChannel == channel)
                {
                  var background = channel == _activeChannel ? ActiveChannelColor : Color.FromArgb(51, 51, 51);
                  using (var brush = new SolidBrush(Helper.Mix(Color.Lime, background, _opacity[channel])))
                    g.FillRectangle(brush, box);
                }

                channel++;
                g.DrawString(channel.ToString(), fontMini, brushForeground, box, stringFormat);
              }
          }
        }
      }
    }

    private void MidiBus_SizeChanged(object sender, EventArgs e)
    {
      Invalidate();
    }

    private void MidiBus_LocationChanged(object sender, EventArgs e)
    {
      Invalidate();
    }
  }
}
