namespace GF.Data.Entity
{
    /// <summary>
    /// 配置表对象
    /// </summary>
    public class SettingItem
    {
        //参数列表
        private SettingBasicItem _basic = new SettingBasicItem();
        private SettingParameterItem _parameter = new SettingParameterItem();

        /// <summary>
        /// 获取或设置基本功能
        /// </summary>
        public SettingBasicItem Basic
        {
            get { return this._basic; }
            set { this._basic = value; }
        }

        /// <summary>
        /// 获取或设置参数
        /// </summary>
        public SettingParameterItem Parameter
        {
            get { return this._parameter; }
            set { this._parameter = value; }
        }
    }
}
