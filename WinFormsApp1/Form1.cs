using System.Linq;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private TextBox _textBox1;
        private int[,] _gameField;
        private Size _buttonSize = new Size(50, 50);
        private int _n;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Pilot Brothers Safe";
            Init();
            
        }
        private void Init()
        {
            this.Controls.Clear();
            this.Width = 500;
            this.Height = 700;
            TextBox textBox = new TextBox();
            textBox.Location = new Point(10, 50);
            textBox.Size = new Size(200, 400);
            _textBox1 = textBox;
            this.Controls.Add(textBox);
            Button button = new Button();
            button.Location = new Point(10, 90);
            button.Size = new Size(50, 50);
            button.Text = "OK";
            button.Click += new EventHandler(Start);
            this.Controls.Add(button);
            Label label = new Label();
            label.Text = "¬ведите целое число";
            label.Size = new Size(200, 200);
            label.Location = new Point(10, 10);
            this.Controls.Add(label);
        }
        private void Start(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_textBox1.Text))
            {
                if (int.TryParse(_textBox1.Text, out int numericValue) && (numericValue > 0))
                {
                    CreateTable(numericValue);
                }
                else
                {
                    _textBox1.Text = "введите целое положительное число";
                }
            }
        }
        private void CreateTable(int n)
        {
            _n = n;
            _gameField = new int[n,n];
            Random random = new Random();
            for(int i = 0; i<100; i++)
            {
                int x = random.Next() % n;
                int y = random.Next() % n;
                _gameField[x,y] = (_gameField[x, y]+1)% 2;
                if(x-1 >= 0)
                {
                    Swap(x - 1, y);
                }
                if (x + 1 < n)
                {
                    Swap(x + 1, y);
                }
                if (y - 1 >= 0)
                {
                    Swap(x, y-1);
                }
                if (y + 1 < n)
                {
                    Swap(x, y + 1);
                }
            }
            this.Controls.Clear();
            this.Width = Math.Max((_buttonSize.Width + 10) * n, 200);
            this.Height = Math.Max((_buttonSize.Height + 10) * n ,200);
            for(int i = 0; i < n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    Button button = new Button();
                    button.Size = _buttonSize;
                    button.Location = new Point(5+i*_buttonSize.Width, 5 + j * _buttonSize.Height);
                    button.Name = i + " " + j;
                    SetTextOnButton(button, _gameField[i, j]);
                    button.Click += new EventHandler(ClickOnButton);
                    this.Controls.Add(button);
                }
            }
        }
        private void ClickOnButton(object sender, EventArgs e)
        {
            Button buttonClicked = (Button)sender;
            int i = int.Parse(buttonClicked.Name.Split(" ")[0]);
            int j = int.Parse(buttonClicked.Name.Split(" ")[1]);
            SetTextOnButton(buttonClicked, Swap(i, j));
            FindAllAround(i, j).ForEach(x => SetTextOnButton((Button)x, Swap(((Button)(x)).Name)));
            CheckWin();
        }

        private int Swap(int i, int j)
        {
            _gameField[i, j] = (_gameField[i, j] + 1) % 2;
            return _gameField[i, j];
        }
        private int Swap(string s)
        {
            return Swap(int.Parse(s.Split(" ")[0]), int.Parse(s.Split(" ")[1]));
        }
        private void SetTextOnButton(Button button, int i)
        {
            if (i == 0)
            {
                button.Text = "|";
            }
            else
            {
                button.Text = "-";
            }
        }
        private List<object> FindAllAround(int i, int j)
        {
            var list = new List<object>();
            list.AddRange((IEnumerable<object>)this.Controls.Find((i-1).ToString()+" "+j, true));
            list.AddRange((IEnumerable<object>)this.Controls.Find((i + 1).ToString() + " " + j, true));
            list.AddRange((IEnumerable<object>)this.Controls.Find(i+ " " + (j-1).ToString(), true));
            list.AddRange((IEnumerable<object>)this.Controls.Find(i+ " " + (j+1).ToString(), true));
            return list;
        }
        private void CheckWin()
        {
            int sum = 0;
            for(int i = 0; i < _n; i++)
            {
                for(int j = 0; j< _n; j++)
                {
                    sum += _gameField[i, j];
                }
            }
            if ((sum == 0)| (sum == _n * _n))
            {
                Win();
            }
        }
        private void Win()
        {
            Label label = new Label();
            label.BackColor = Color.Red;
            label.Text = "WIN!!!";
            label.Size = new Size(this.Width, this.Height);
            label.Location = new Point(0, 0);
            this.Controls.Clear();
            this.Controls.Add(label);
            this.Update();
            Restart();
        }

        private void Restart()
        {
            Thread.Sleep(10000);
            Init();
        }

    }
}