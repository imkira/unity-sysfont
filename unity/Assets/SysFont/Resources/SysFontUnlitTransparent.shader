Shader "SysFont/Unlit Transparent" 
{
  Properties 
  {
    _Color ("Text Color", Color) = (1,1,1,1)    
    _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
  }

  SubShader
  {
    Tags
    {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType" = "Transparent"
    }

    Pass 
    {
      Cull Off
      Lighting Off
      ZWrite Off
      Fog { Mode Off }
      AlphaTest Off
      Blend SrcAlpha OneMinusSrcAlpha

      SetTexture [_MainTex] 
      {
        ConstantColor [_Color]
        Combine constant, texture * constant
      }
    }
  }
}
