using ModuleA.Models;
using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleB.ViewModels
{
    public class ModuleBViewModel : BindableBase
    {
        public string Title => "ModuleBView";

        private string lhs;

        public string Lhs
        {
            get { return this.lhs; }
            set { this.SetProperty(ref this.lhs, value); }
        }

        private string rhs;

        public string Rhs
        {
            get { return this.rhs; }
            set { this.SetProperty(ref this.rhs, value); }
        }

        private int answer;

        public int Answer
        {
            get { return this.answer; }
            set { this.SetProperty(ref this.answer, value); }
        }

        public DelegateCommand AddCommand { get; }

        public DelegateCommand LoadModuleCCommand { get; }

        public ModuleBViewModel(Calc calc, IModuleManager moduleManager)
        {
            this.AddCommand = new DelegateCommand(
                () => this.Answer = calc.Add(int.Parse(this.Lhs), int.Parse(this.Rhs)),
                () =>
                {
                    var temp = 0;
                    if (!int.TryParse(this.Lhs, out temp))
                    {
                        return false;
                    }

                    return int.TryParse(this.Rhs, out temp);
                })
                .ObservesProperty(() => this.Lhs)
                .ObservesProperty(() => this.Rhs);

            this.LoadModuleCCommand = new DelegateCommand(() => moduleManager.LoadModule("ModuleCModule"));
        }
    }
}
