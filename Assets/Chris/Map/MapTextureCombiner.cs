// ==================================================================== \\
// File   : MapTextureCombiner.cs                                       \\
// Author : Christopher Stamford                                        \\
//                                                                      \\
// Blends two textures together. Unused due to speed concerns.          \\
// ==================================================================== \\

using UnityEngine;

namespace cst.Map
{
    public class MapTextureCombiner : MonoBehaviour
    {
        [SerializeField] private Texture       m_baseTexture;
        [SerializeField] private RenderTexture m_renderTexture;
        [SerializeField] private Vector2       m_topLeft = new Vector2(1190, 500);
        [SerializeField] private Vector2       m_size = new Vector2(858, 1548);

        private Texture2D m_renderTextureReadable;
        private Texture2D m_unmodifiedBaseTexture;
        private Texture2D m_modifiedTexture;

        public void Start()
        {
            Texture2D m_unmodifiedBaseTexture = m_baseTexture as Texture2D;

            m_modifiedTexture = new Texture2D(m_unmodifiedBaseTexture.width, m_unmodifiedBaseTexture.height);
        }

        public void Update()
        {
            Color[] rawColours = m_modifiedTexture.GetPixels();

            int lowBoundX = (int)m_topLeft.x;
            int highBoundX = (int)m_topLeft.x + (int)m_size.x;

            // For some reason, the texture is upside-down
            int lowBoundY = 0;
            int highBoundY = (int)m_size.y;

            for (int x = lowBoundX; x < highBoundX; ++x)
            {
                for (int y = lowBoundY; y < highBoundY; ++y)
                {
                    rawColours[x + (y * m_modifiedTexture.width)] = Color.red;
                }
            }

            m_modifiedTexture.SetPixels(rawColours);
            m_modifiedTexture.Apply();
            renderer.material.mainTexture = m_modifiedTexture;
        }
    }
}