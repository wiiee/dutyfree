namespace DutyFree.Platform.Model.BootstrapFileInput
{
    using System.Collections.Generic;
    using System.Linq;

    public class SendingData
    {
        public List<string> initialPreviews { get; set; }
        public List<InitialPreviewConfig> initialPreviewConfigs { get; set; }

        public SendingData(List<InitialPreview> initialPreviews, List<InitialPreviewConfig> initialPreviewConfigs)
        {
            this.initialPreviewConfigs = initialPreviewConfigs;
            this.initialPreviews = initialPreviews.Select(o => o.GetPreview()).ToList();
        }
    }
}
