using System;
using ConsoleGame;
using ConsoleGame.Behaviour;

class Heart : ObjectBehaviour
{
    float speed = 20f;
    float TimeInSecondsToClose = 5f;
    public bool DestroyMe = false;

    public bool AllowMovement = false;

    public override void Awake()
    {
        gameObject.transform.position = new Vector2(Screen.Width / 2 - 1, Screen.Height - 10);
        Screen.AppendChar(9, gameObject.transform.position, '♥', ConsoleColor.Red);
    }

    public override void Update()
    {
        Screen.AppendChar(9, gameObject.transform.position, ' ');

        if (AllowMovement)
        {
            //Screen.ClearLayer(8);
            //Screen.AppendString(8, new Vector2(0, 0), $"Debug (combat): Time left: {TimeInSecondsToClose}");
            if (TimeInSecondsToClose <= 0f) DestroyMe = true;
            if (Input.GetKeyDown(ConsoleKey.LeftArrow))
            {
                if (!Screen.IsColliding(6, gameObject.transform.position + (Vector2.left * Time.deltaTime * speed)))
                    gameObject.transform.translate(Vector2.left * Time.deltaTime * speed);
            }
            if (Input.GetKeyDown(ConsoleKey.UpArrow))
            {
                if (!Screen.IsColliding(6, gameObject.transform.position + (Vector2.up * Time.deltaTime * speed * 2f)))
                    gameObject.transform.translate(Vector2.up * Time.deltaTime * speed * 2f);
            }
            if (Input.GetKeyDown(ConsoleKey.RightArrow))
            {
                if (!Screen.IsColliding(6, gameObject.transform.position + (Vector2.right * Time.deltaTime * speed)))
                    gameObject.transform.translate(Vector2.right * Time.deltaTime * speed);
            }

            if (!Screen.IsColliding(6, gameObject.transform.position + (Vector2.down * Time.deltaTime * (speed/2))))
                gameObject.transform.translate(Vector2.down * Time.deltaTime * (speed/2));

            TimeInSecondsToClose -= Time.deltaTime / 10;
        }
        if (Visible)
            Screen.AppendChar(9, gameObject.transform.position, '♥', ConsoleColor.Red);
    }

    public override void OnDelete()
    {
        Screen.AppendChar(9, gameObject.transform.position, ' ');
        Screen.ClearLayer(8);
    }
}