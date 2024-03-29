﻿#define CG_Gizmo

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
  class Mundo : GameWindow
  {
    private static Mundo instanciaMundo = null;

    private Mundo(int width, int height) : base(width, height) { }

    public static Mundo GetInstance(int width, int height)
    {
      if (instanciaMundo == null)
        instanciaMundo = new Mundo(width, height);
      return instanciaMundo;
    }

    private CameraOrtho camera = new CameraOrtho();
    protected List<Objeto> objetosLista = new List<Objeto>();
    public ObjetoGeometria objetoSelecionado = null;
    private bool bBoxDesenhar = false;
    int mouseX, mouseY;
    private bool mouseMoverPto = false;

    private List<PrimitiveType> lista_primitivas = new List<PrimitiveType>()
    {
      PrimitiveType.Points,
      PrimitiveType.Lines,
      PrimitiveType.LineLoop,
      PrimitiveType.LineStrip,
      PrimitiveType.Triangles,
      PrimitiveType.TriangleStrip,
      PrimitiveType.TriangleFan,
      PrimitiveType.Quads,
      PrimitiveType.QuadStrip,
      PrimitiveType.Polygon
    };

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");

      GL.ClearColor(Color.Gray);
      
      camera.xmin = -300;
      camera.xmax = 300;
      camera.ymin = -300;
      camera.ymax = 300;
    }

    public void addObjeto(Objeto o)
    {
      objetosLista.Add(o);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();

      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }

      if (bBoxDesenhar && (objetoSelecionado != null))
      {
        objetoSelecionado.BBox.Desenhar();
      }

      this.SwapBuffers();
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      if (e.Key == Key.H)
      {
        Utilitario.AjudaTeclado();
      }
      else if (e.Key == Key.Escape)
      {
        Exit();
      }
      else if (e.Key == Key.E)
      {
        camera.xmin -= 10;
        camera.xmax -= 10;
      }
      else if (e.Key == Key.D)
      {
        camera.xmin += 10;
        camera.xmax += 10;
      }
      else if (e.Key == Key.B)
      {
        camera.ymin -= 10;
        camera.ymax -= 10;
      }
      else if (e.Key == Key.C)
      {
        camera.ymin += 10;
        camera.ymax += 10;
      }
      else if (e.Key == Key.I)
      {
        if (camera.ymax - camera.ymin > 175)
        {
          camera.ymin += 10;
          camera.ymax -= 10;
          camera.xmin += 10;
          camera.xmax -= 10;
        }
      }
      else if (e.Key == Key.O)
      {
        camera.ymin -= 10;
        camera.ymax += 10;
        camera.xmin -= 10;
        camera.xmax += 10;
      }
      else if (e.Key == Key.Space)
      {
        if (objetoSelecionado != null)
        {
          int atual = lista_primitivas.IndexOf(objetoSelecionado.PrimitivaTipo);
          objetoSelecionado.PrimitivaTipo = lista_primitivas[(atual + 1) % lista_primitivas.Count];
        }
      }
      else if (e.Key == Key.Z)
      {
        if (objetoSelecionado != null)
        {
          objetoSelecionado.Angulo = (objetoSelecionado.Angulo + 9) % 360;
        }
      }
      else if (e.Key == Key.X)
      {
        if (objetoSelecionado != null)
        {
          objetoSelecionado.Angulo = (objetoSelecionado.Angulo - 9) % 360;
        }
      }
      else if (e.Key == Key.A)
      {
        if (objetoSelecionado != null)
        {
          objetoSelecionado.PrimitivaTamanho += 0.1f;
        }
      }
      else if (e.Key == Key.S)
      {
        if (objetoSelecionado != null)
        {
          objetoSelecionado.PrimitivaTamanho -= 0.1f;
        }
      }
      else if (e.Key == Key.V)
      {
        mouseMoverPto = !mouseMoverPto;
      }
      else if (e.Key == Key.Q)
      {
        if (objetoSelecionado != null)
        {
          objetoSelecionado.Posicao += new Ponto4D(-10, 0);
        }
      }
      else if (e.Key == Key.W)
      {
        if (objetoSelecionado != null)
          objetoSelecionado.Posicao += new Ponto4D(10, 0);
        {
        }
      }
      else
      {
        Console.WriteLine(" __ Tecla não implementada.");
      }
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X;
      mouseY = 600 - e.Position.Y;
      if (mouseMoverPto && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosUltimo().X = mouseX;
        objetoSelecionado.PontosUltimo().Y = mouseY;
      }
    }
  }

}
