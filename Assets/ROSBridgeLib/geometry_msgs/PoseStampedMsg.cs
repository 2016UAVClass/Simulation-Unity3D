using System.Collections;
using System.Text;
using SimpleJSON;
using ROSBridgeLib.std_msgs;

namespace ROSBridgeLib {
	namespace geometry_msgs {
		public class PoseStampedMsg : ROSBridgeMsg {
			public HeaderMsg _header;
			public PoseMsg _pose;
			
			public PoseStampedMsg(JSONNode msg) {
				_header = new HeaderMsg(msg["header"]);
				_pose = new PoseMsg(msg["pose"]);
			}
/* 			
			public static string GetMessageType() {
				return "geometry_msgs/Twist";
			}
			
			public Vector3Msg GetLinear() {
				return _linear;
			}

			public Vector3Msg GetAngular() {
				return _angular;
			}
			
			public override string ToString() {
				return "Twist [linear=" + _linear.ToString() + ",  angular=" + _angular.ToString() + "]";
			}
			
			public override string ToYAMLString() {
				return "{\"linear\" : " + _linear.ToYAMLString() + ", \"angular\" : " + _angular.ToYAMLString() + "}";
			} */
		}
	}
}