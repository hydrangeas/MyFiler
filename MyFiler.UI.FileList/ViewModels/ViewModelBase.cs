using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.UI.FileList.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        public virtual void Update()
        {
        }

        public virtual Guid GetNewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
