using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class XnaContentManagerAdapter : IContentLoader
    {
        private ContentManager contentManager;

        public XnaContentManagerAdapter(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }
        public T Load<T>(string contentId)
        {
            return contentManager.Load<T>(contentId);
        }

        public void Unload()
        {
            contentManager.Unload();
        }
    }
}
