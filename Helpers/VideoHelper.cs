using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace fastforward.Helpers
{
    public class VideoHelper
    {
        public static int PartnerId = Convert.ToInt32(ConfigurationManager.AppSettings["KALTURA_PARTNER_ID"]);

        private const string KALTURA_DOWNLOAD_URL = "http://www.kaltura.com/p/{0}/sp/0/playManifest/entryId/{1}/format/url/flavorParamId/0";

        public static string PlayerId = "12437621";
        public static string KalturaScript = "<script type=\"text/javascript\" src=\"https://www.kaltura.com/p/{4}/sp/{4}00/embedIframeJs/uiconf_id/{5}/partner_id/{4}\"></script>"

            + "		<object  "
                + "		  id=\"{0}\"  "
                + "		  name=\"{0}\"  "
                + "		  type=\"application/x-shockwave-flash\"  "
                + "		  height=\"{1}\"  "
                + "		  width=\"{2}\"  "

                + "		  allowFullScreen=\"{3}\"  "
                + "		  allowNetworking=\"all\"  "
                + "		  allowScriptAccess=\"always\"  "
                + "		  data=\"https://www.kaltura.com/kwidget/wid/_{4}/uiconf_id/{5}/entry_id/{6}\"  "
                + "		  xmlns:dc=\"http://purl.org/dc/terms/\"  "
                + "		  xmlns:media=\"http://search.yahoo.com/searchmonkey/media/\"  "
                + "		  rel=\"media:video\"  "
                + "		  resource=\"https://www.kaltura.com/kwidget/wid/_{4}/uiconf_id/{5}/entry_id/{6}\" />"
                + "		  <param name=\"allowFullScreen\" value=\"true\" /> "
                + "		  <param name=\"allowNetworking\" value=\"all\" /> "
                + "		  <param name=\"allowScriptAccess\" value=\"always\" /> "
                + "		  <param name=\"bgcolor\" value=\"{7}\" /> "
                + "		  <param name=\"flashVars\" value=\"\" /> "
                + "       <param name=\"wmode\" value=\"opaque\" /> "
                + "		  <param name=\"movie\" value=\"https://www.kaltura.com/kwidget/wid/_{4}/uiconf_id/{5}/entry_id/{6}\" /> "
                + "		   <!--You can enter alternate content here. --> "
                + "		</object> ";

        public static string GetVideo(string videoId, string playerId = "")
        {
            if (string.IsNullOrEmpty(playerId))
            {
                playerId = PlayerId;
            }
            return GetVideo(playerId, videoId, 300, 550);

        }
        public static string GetVideo(string playerId, string videoId, int height, int width)
        {

            return GetVideo(playerId, videoId, height, width, null);

        }
        public static string GetVideo(string playerId, string videoId, int height, int width, string uniqueObjId, string bgcolor = "#000000", bool allowFullScreen = true)
        {
            if (string.IsNullOrEmpty(uniqueObjId))
            {
                uniqueObjId = Guid.NewGuid().ToString("N");
            }

            return string.Format(KalturaScript,
                                            uniqueObjId,
                                            height,
                                            width,
                                            allowFullScreen,
                                            PartnerId,
                                            playerId,
                                            videoId,
                                            bgcolor
                                );

        }

        public static string GetVideoDownloadUrl(string videoId)
        {

            return string.Format(KALTURA_DOWNLOAD_URL, PartnerId, videoId);
        }
    }
}