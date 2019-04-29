using UnityEngine;

public class DemoInputObserver : MonoBehaviour {

  #region Unity magic methods
  protected void OnEnable() {
    GameInputManager.ObserveKeyCode(KeyCode.Space);
    GameInputManager.ObserveKeyCode(KeyCode.Escape);
    GameInputManager.ObserveAxis("Horizontal");

    GameInputManager.Register(OnInputEvent);
  }

  protected void OnDisable() {
    GameInputManager.Unregister(OnInputEvent);
  }
  #endregion

  #region Internals (under the hood)
  protected void OnInputEvent(GameInputManager.EventData data) {
    if (data.used) return;

    if (data.keyCode==KeyCode.Space) {
      Debug.Log("Spacebar was pressed");
      data.used = true;
    } else if (data.keyCode==KeyCode.Escape) {
      Debug.Log("Escape was pressed");
      data.used = true;
    } else if (data.axis=="Horizontal") {
      if (data.value!=0f) {
        Debug.Log("Horizontal axis = " + data.value.ToString());
      }
      data.used = true;
    }
  }
  #endregion
}