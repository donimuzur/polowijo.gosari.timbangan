using polowijo.gosari.timbangan.dal;
using polowijo.gosari.timbangan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polowijo.gosari.timbangan
{
    public class BaseController
    {
        public static Login CurrentUser { set; get; }
    }
    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
