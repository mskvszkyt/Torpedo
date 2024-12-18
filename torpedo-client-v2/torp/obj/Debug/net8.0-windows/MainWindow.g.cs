﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "37F77A446CBA726C8B7F4C56152ECE6EA0B57D62"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ShipPlacement {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOrientation;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LeftGrid;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RightGrid;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReadyButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/torp;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 5 "..\..\..\MainWindow.xaml"
            ((ShipPlacement.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 25 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Ship_MouseMove);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 27 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Ship_MouseMove);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 29 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Ship_MouseMove);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 31 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Ship_MouseMove);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 33 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Ship_MouseMove);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lblOrientation = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.LeftGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 39 "..\..\..\MainWindow.xaml"
            this.LeftGrid.Drop += new System.Windows.DragEventHandler(this.LeftGrid_Drop);
            
            #line default
            #line hidden
            return;
            case 9:
            this.RightGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            
            #line 94 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 95 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 96 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 97 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 98 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 99 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 100 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 101 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 102 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 103 "..\..\..\MainWindow.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Cell_MouseDown);
            
            #line default
            #line hidden
            return;
            case 20:
            this.ReadyButton = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\MainWindow.xaml"
            this.ReadyButton.Click += new System.Windows.RoutedEventHandler(this.ReadyButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

