using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class ColorModelAccessor
    {
        public ColorModel ColorModel
        {
            get
            {
                return ColorModel.Instance;
            }
        }
    }
}
