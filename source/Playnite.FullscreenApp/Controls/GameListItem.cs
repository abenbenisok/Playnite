﻿using Playnite.Commands;
using Playnite.Common;
using Playnite.Controls;
using Playnite.Converters;
using Playnite.FullscreenApp.ViewModels;
using Playnite.Extensions;
using Playnite.SDK;
using Playnite.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BooleanToVisibilityConverter = Playnite.Converters.BooleanToVisibilityConverter;

namespace Playnite.FullscreenApp.Controls
{
    [TemplatePart(Name = "PART_ImageCover", Type = typeof(Image))]
    public class GameListItem : Control
    {
        private readonly FullscreenAppViewModel mainModel;
        private Image ImageCover;

        static GameListItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameListItem), new FrameworkPropertyMetadata(typeof(GameListItem)));
        }

        public GameListItem() : this(FullscreenApplication.Current?.MainModel)
        {
        }

        public GameListItem(FullscreenAppViewModel mainModel)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.mainModel = DesignMainViewModel.DesignIntance;
            }
            else if (mainModel != null)
            {
                this.mainModel = mainModel;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ImageCover = Template.FindName("PART_ImageCover", this) as Image;
            if (ImageCover != null)
            {
                var sourceBinding = new PriorityBinding();
                sourceBinding.Bindings.Add(new Binding()
                {
                    Path = new PropertyPath(nameof(GamesCollectionViewEntry.CoverImageObject)),
                    IsAsync = mainModel.AppSettings.Fullscreen.AsyncImageLoading,
                    Converter = new NullToDependencyPropertyUnsetConverter(),
                    Mode = BindingMode.OneWay
                });
                sourceBinding.Bindings.Add(new Binding()
                {
                    Path = new PropertyPath(nameof(GamesCollectionViewEntry.DefaultCoverImageObject)),
                    Converter = new NullToDependencyPropertyUnsetConverter(),
                    Mode = BindingMode.OneWay
                });

                BindingOperations.SetBinding(ImageCover, Image.SourceProperty, sourceBinding);
            }

            ControlTemplateTools.InitializePluginControls(
                mainModel.Extensions,
                Template,
                this,
                ApplicationMode.Fullscreen,
                this.DataContext,
                nameof(GamesCollectionViewEntry.Game));
        }
    }
}