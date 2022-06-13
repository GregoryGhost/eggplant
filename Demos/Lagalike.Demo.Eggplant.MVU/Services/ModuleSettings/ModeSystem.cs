namespace Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings
{
    /// <inheritdoc />
    public class ModeSystem : BaseModeSystem
    {
        /// <inheritdoc />
        public ModeSystem(CockSizerInfo modeInfo, HandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}