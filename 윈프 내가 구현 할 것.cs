using System;
using System.Drawing;
using System.Windows.Forms;

namespace 윈프_충돌_이동_구현
{
    public partial class Form1 : Form
    {
        // 게임 기본 변수 선언
        int gameSpeed = 6;
        int avoidCount = 0;     // 장애물을 피한 횟수 (속도 증가용 카운트)

        public Form1()
        {
            InitializeComponent();

            // 키 입력 감지 설정
            this.KeyPreview = true;

            // 타이머 주기 설정 (20ms)
            timer1.Interval = 20;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 폼 로드 시 초기 설정
        }

        // 게임 시작 버튼 클릭 시
        private void button1_Click(object sender, EventArgs e)
        {
            // 게임이 시작되면 시작 버튼 숨기기
            button1.Visible = false;

            // 버튼에 남아있는 포커스를 해제하여 스페이스바가 버튼을 누르는 것으로 오인되는 현상 방지
            this.ActiveControl = null;

            // 리소스 이미지 연결
            try
            {
                pbPlayer.Image = Properties.Resources.player_run1;
                pbObstacle.Image = Properties.Resources.paper;
            }
            catch
            {
                // 예외 처리
            }

            // 이미지 비율 모드 및 크기 설정 (하반신 잘림 방지: 세로 80으로 넉넉하게 지정)
            pbPlayer.SizeMode = PictureBoxSizeMode.Zoom;
            pbObstacle.SizeMode = PictureBoxSizeMode.Zoom;

            pbPlayer.Size = new Size(60, 80);
            pbObstacle.Size = new Size(50, 60);

            // 장애물 초기 위치 세팅 (오른쪽 끝)
            pbObstacle.Left = this.ClientSize.Width + 50;
            pbObstacle.Top = pbPlayer.Top;

            // 변수 초기화 및 타이머 시작
            avoidCount = 0;
            gameSpeed = 6;
            timer1.Start();
        }

        // 게임 루프 타이머 틱 이벤트
        private void timer1_Tick(object sender, EventArgs e)
        {
            // 1. 장애물 이동
            pbObstacle.Left -= gameSpeed;

            // 화면 왼쪽 밖으로 나가면 장애물을 오른쪽으로 재배치 (회피 성공)
            if (pbObstacle.Right < 0)
            {
                pbObstacle.Left = this.ClientSize.Width + 50;
                avoidCount++;

                // [속도 증가] 장애물을 피할 때마다 속도가 점점 빨라짐 (최대 20 제한)
                if (gameSpeed < 20)
                {
                    gameSpeed = 6 + (avoidCount / 2); // 피할 때마다 점진적으로 속도 상승
                }
            }

            // 2. 충돌 판정 확인
            CheckCollision();
        }

        // 충돌 판정 함수 (.IntersectsWith)
        private void CheckCollision()
        {
            Rectangle playerBox = new Rectangle(
                pbPlayer.Left + 5,
                pbPlayer.Top + 5,
                pbPlayer.Width - 10,
                pbPlayer.Height - 10
            );

            Rectangle obstacleBox = new Rectangle(
                pbObstacle.Left + 5,
                pbObstacle.Top + 5,
                pbObstacle.Width - 10,
                pbObstacle.Height - 10
            );

            if (playerBox.IntersectsWith(obstacleBox))
            {
                timer1.Stop();

                // 장애물에 부딪히는 순간 충돌 이미지로 변경
                pbPlayer.Image = Properties.Resources.player_collision;

                MessageBox.Show("과제 폭탄을 맞았습니다．게임 오버", "School Escape");

                // 게임 오버 시 시작 버튼 다시 보이기
                button1.Visible = true;
                ResetGame();
            }
        }

        // 게임 초기화 함수
        private void ResetGame()
        {
            gameSpeed = 6;
            avoidCount = 0;
            pbObstacle.Left = this.ClientSize.Width + 50;
        }
    }
}