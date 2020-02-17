Shader "Depth Mask Simple (Terrain)" {
  SubShader {
    Lighting Off
    ZTest LEqual
    ZWrite On
    ColorMask 0
    Pass {}
  }
}