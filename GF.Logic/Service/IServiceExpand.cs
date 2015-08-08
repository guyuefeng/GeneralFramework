namespace GF.Logic.Service
{
    public interface IServiceExpand
    {
        /// <summary>
        /// 关键字，文件名：Service.aspx?act=expand&amp;key=关键字
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 写出内容（可进行写出所有内容）
        /// </summary>
        void OutWrite();
    }
}
