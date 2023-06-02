using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 异步刷新
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
