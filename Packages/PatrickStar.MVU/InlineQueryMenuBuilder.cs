namespace PatrickStar.MVU
{
    using System.Collections.Generic;
    
    public class InlineQueryMenuBuilder
    {
        private readonly List<InlineQueryButton> _buttons = new();
        
        public InlineQueryMenu Build()
        {
            var menu = new InlineQueryMenu
            {
                Buttons = _buttons.ToArray()
            };

            return menu;
        }
        
        public InlineQueryMenuBuilder Button(InlineQueryButton btn)
        {
            _buttons.Add(btn);

            return this;
        }
    }
}