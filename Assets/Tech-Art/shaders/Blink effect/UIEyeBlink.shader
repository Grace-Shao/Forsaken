Shader "Custom/UIEyeBlink"
{
    Properties
    {
        // Kept for uGUI compatibility (Image assigns its sprite texture here);
        // we deliberately don't sample it since this is a solid-color eyelid mask.
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color            ("Tint", Color)                 = (1,1,1,1)
        [HDR] _VignetteColor ("Eyelid Color", Color)      = (0,0,0,1)
        _BlinkAmount      ("Blink Amount", Range(0,1))    = 0

        _EyeWidth         ("Eye Half-Width", Range(0.2, 3))     = 1.3
        _EyeMaxHeight     ("Eye Open Half-Height", Range(0.5, 2)) = 1.0
        _CurveExponent    ("Corner Sharpness", Range(0.2, 3))    = 0.5
        _Softness         ("Edge Softness", Range(0.001, 0.3))   = 0.03

        // Standard uGUI masking boilerplate so this plays nicely under a Mask/RectMask2D.
        _StencilComp      ("Stencil Comparison", Float) = 8
        _Stencil          ("Stencil ID", Float)         = 0
        _StencilOp        ("Stencil Operation", Float)  = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask  ("Stencil Read Mask", Float)  = 255
        _ColorMask        ("Color Mask", Float)         = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                fixed4 color         : COLOR;
                float2 texcoord      : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _VignetteColor;
            float  _BlinkAmount;
            float  _EyeWidth;
            float  _EyeMaxHeight;
            float  _CurveExponent;
            float  _Softness;
            float4 _ClipRect;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex        = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord      = IN.texcoord;
                OUT.color         = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Centre the UVs and aspect-correct X so the eye shape doesn't stretch on wide screens.
                float2 centered = (IN.texcoord - 0.5) * 2.0; // -1..1 across the overlay
                centered.x *= _ScreenParams.x / max(_ScreenParams.y, 1.0);

                float halfWidth = max(_EyeWidth, 0.0001);
                float xN        = saturate(abs(centered.x) / halfWidth);
                // Ellipse-like arc: 1 at screen centre, tapering to 0 at the eye's corners.
                float curve     = pow(saturate(1.0 - xN * xN), _CurveExponent);

                // The eye's open half-height shrinks toward 0 as the blink amount rises to 1.
                float openHeight = _EyeMaxHeight * (1.0 - _BlinkAmount);
                float boundary   = openHeight * curve;

                float dist   = abs(centered.y) - boundary;          // negative = inside the open eye
                float eyelid = smoothstep(-_Softness, _Softness, dist);

                // Force full closure past the eye's horizontal reach so no seam shows at the corners.
                float outsideX = smoothstep(halfWidth, halfWidth + _Softness, abs(centered.x));
                eyelid = max(eyelid, outsideX);

                // At dist == 0 (the exact boundary) smoothstep always returns 0.5, not 0 or 1.
                // When the eye is fully open that 0.5 line is a single negligible pixel, but once
                // the eye fully shuts the boundary collapses to a flat line across the WHOLE screen
                // width at y=0, turning that single-pixel artifact into a full-width pale seam.
                // Explicitly ramp to full opacity over the last bit of the close so it never shows.
                float closeBias = smoothstep(1.0 - _Softness, 1.0, _BlinkAmount);
                eyelid = max(eyelid, closeBias);

                fixed4 col = _VignetteColor * IN.color;
                col.a *= eyelid;

                #ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                return col;
            }
            ENDCG
        }
    }
}
