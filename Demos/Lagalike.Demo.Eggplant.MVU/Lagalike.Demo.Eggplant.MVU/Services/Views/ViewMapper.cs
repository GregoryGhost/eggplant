namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class ViewMapper : BaseMainViewMapper<CommandTypes, ModelTypes>
    {
        /// <inheritdoc />
        public ViewMapper(ViewsFactory viewsFactory)
            : base(viewsFactory.GetViews())
        {
        }

        public override IView<CommandTypes> Map(IModel model)
        {
            var rootModel = (Model) model;
            var view = rootModel.CurrentCommand switch
            {
                CommandTypes.ShareCockSize => GetModelInterface(rootModel.CockSizeModel),
                CommandTypes.GroupRating => GetModelInterface(rootModel.GroupRatingModel),
                _ => throw new ArgumentOutOfRangeException(nameof(rootModel.CurrentCommand), $"Cannot match unknown current command type {rootModel.CurrentCommand}")
            };
            
            return view;
        }

        private IView<CommandTypes> GetModelInterface(IModel? model)
        {
            ThrowIfModelIsNull(model);

            var view = base.Map(model);

            return view;
        }

        private static void ThrowIfModelIsNull([NotNull]IModel? rootModelGroupRatingModel)
        {
            if (rootModelGroupRatingModel is null)
                throw new ArgumentException("You should initialize model", nameof(rootModelGroupRatingModel));
        }
    }
}