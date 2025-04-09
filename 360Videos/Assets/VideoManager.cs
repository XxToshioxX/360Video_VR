using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO.Ports;
using System.IO;
using System.Threading;
using UnityEngine.UI;



public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    private VideoPlayer videoPlayer;

    public static string ArdPort = "COM9";
    public static int ArdVel = 9600;

    SerialPort porta = new SerialPort(ArdPort, 9600);

    public GameObject UI_Obj;
    private bool alt;

    public Text txt;

    public bool comeco = true;
    public float time;
    public float Temp = 92f;
    public static bool stoptimer;
    public bool contador;

    //------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        alt = false;
        comeco = true;
        stoptimer = false;

        Temp = 92f;
        time = 0f;

        contador = false;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Habilitar_UI();
        }
        if (porta.IsOpen == true) //Caso ha a conexao com o microcontrolador deve se iniciar o movimento
        {

        }
        if (stoptimer == true)
        {

            //  Tempo2 = Tempo2 + Time.deltaTime;
        }
        if (contador == true)
        {
            time = time + Time.deltaTime;
        }

        if ((time >= 7.4f) && (time <= 7.5f))
        {
            if (porta.IsOpen == true)
            {
                porta.WriteLine("i");
            }
        }

        if (time >= Temp)
        {
            if (porta.IsOpen == true)
            {
                porta.WriteLine("p");
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void Play()
    {
        if (porta.IsOpen == true) //Caso ha conexao com o microcontrolador deve se iniciar o movimento
        {
            if (comeco == true)
            {
                //porta.WriteLine("C"); //Inicio (i)
                comeco = false;
            }
            else
            {
                if (comeco == false)

                    porta.WriteLine("a"); //Inicio (a)
            }

            contador = true;
            // StartCoroutine("contagemRegressiva");
            stoptimer = false;
            videoPlayer.Play();
            videoPlayer.loopPointReached += DoSimethingWhenVideoFinish;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void Pause()
    {
        if (porta.IsOpen == true) //Caso ha a conexao com o microcontrolador deve se iniciar o movimento
        {
            porta.WriteLine("p"); //Pause/fim (p)
        }
        stoptimer = true;
        videoPlayer.Pause();
        contador = false;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void Stop()
    {
        videoPlayer.Stop();
        if (porta.IsOpen == true) //Caso ha a conexao com o microcontrolador deve se iniciar o movimento
        {
            porta.WriteLine("p"); //Pause/fim (p)  
        }
        comeco = true;
        stoptimer = false;


        Temp = 92f;
        time = 0f;
        contador = false;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void URLToVideo(string url)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        Play();
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void Quit()
    {
        Application.Quit();
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void Habilitar_UI()
    {
        if (alt == false)
        {
            UI_Obj.SetActive(true);
            alt = true;
        }
        else
        {
            UI_Obj.SetActive(false);
            alt = false;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void DoSimethingWhenVideoFinish(VideoPlayer vp) //Quando o video acaba
    {

        if (porta.IsOpen == true)
        {
            porta.WriteLine("p");
        }
        stoptimer = false;
        comeco = true;
        contador = false;

        Temp = 92f;
        time = 0f;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    /*
    IEnumerator contagemRegressiva()
    {
        yield return new WaitForSeconds(0);
     
    }
    */

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void ConectarArd() //Funcao que conecta o arduino
    {
        porta = new SerialPort(ArdPort, 9600);
        porta.Open();
        Debug.Log("Arduino Conectado");
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void DesconectarArd() //Funcao que desconecta o arduino
    {

        porta.Close();
        Debug.Log("Arduino Desconectado");
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void URLToArd(string COM) //Digitar Porta Serial
    {
        DesconectarArd();
        ArdPort = COM;
        ConectarArd();
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    public void CommandoArd(string Comando) //Digitar Porta Serial
    {
        if (porta.IsOpen == true)
        {
            porta.WriteLine(Comando);
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------------



}