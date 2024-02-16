#include "imgui.h"
#include "cimgui_node_editor_export.h"

extern "C" {

CIMGUI_NODE_EDITOR_EXPORT ImGuiContext* axNodeEditor_GetCurrentContext() {
    return ImGui::GetCurrentContext();
}

CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SetCurrentContext(ImGuiContext* ctx) {
    ImGui::SetCurrentContext(ctx);
}

}
