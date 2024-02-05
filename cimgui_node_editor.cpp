#define FOR_WRAPPER_IMPL 1
#include "imgui_node_editor.h" // This needs to be included first!
#include "cimgui_node_editor.h"
#include <utility> //std::move 

axNodeEditor_SaveReasonFlags axNodeEditor_operator_pipe(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs) {
	return ax::NodeEditor::operator|(lhs, rhs);
}

axNodeEditor_SaveReasonFlags axNodeEditor_operator_amp(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs) {
	return ax::NodeEditor::operator&(lhs, rhs);
}

void axNodeEditor_SetCurrentEditor(axNodeEditor_EditorContext* ctx) {
	ax::NodeEditor::SetCurrentEditor(ctx);
}

axNodeEditor_EditorContext* axNodeEditor_GetCurrentEditor() {
	return ax::NodeEditor::GetCurrentEditor();
}

axNodeEditor_EditorContext* axNodeEditor_CreateEditor(const axNodeEditor_Config* config) {
	return ax::NodeEditor::CreateEditor(config);
}

void axNodeEditor_DestroyEditor(axNodeEditor_EditorContext* ctx) {
	ax::NodeEditor::DestroyEditor(ctx);
}

const axNodeEditor_Config* axNodeEditor_GetConfig(axNodeEditor_EditorContext* ctx) {
	return &ax::NodeEditor::GetConfig(ctx);
}

axNodeEditor_Style* axNodeEditor_GetStyle() {
	return &ax::NodeEditor::GetStyle();
}

const char* axNodeEditor_GetStyleColorName(axNodeEditor_StyleColor colorIndex) {
	return ax::NodeEditor::GetStyleColorName(colorIndex);
}

void axNodeEditor_PushStyleColor(axNodeEditor_StyleColor colorIndex, const ImVec4* color) {
	ax::NodeEditor::PushStyleColor(colorIndex, *color);
}

void axNodeEditor_PopStyleColor(int count) {
	ax::NodeEditor::PopStyleColor(count);
}

void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, float value) {
	ax::NodeEditor::PushStyleVar(varIndex, value);
}

void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, const ImVec2* value) {
	ax::NodeEditor::PushStyleVar(varIndex, *value);
}

void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, const ImVec4* value) {
	ax::NodeEditor::PushStyleVar(varIndex, *value);
}

void axNodeEditor_PopStyleVar(int count) {
	ax::NodeEditor::PopStyleVar(count);
}

void axNodeEditor_Begin(const char* id, const ImVec2* size) {
	ax::NodeEditor::Begin(id, *size);
}

void axNodeEditor_End() {
	ax::NodeEditor::End();
}

void axNodeEditor_BeginNode(axNodeEditor_NodeId id) {
	ax::NodeEditor::BeginNode(id);
}

void axNodeEditor_BeginPin(axNodeEditor_PinId id, axNodeEditor_PinKind kind) {
	ax::NodeEditor::BeginPin(id, kind);
}

void axNodeEditor_PinRect(const ImVec2* a, const ImVec2* b) {
	ax::NodeEditor::PinRect(*a, *b);
}

void axNodeEditor_PinPivotRect(const ImVec2* a, const ImVec2* b) {
	ax::NodeEditor::PinPivotRect(*a, *b);
}

void axNodeEditor_PinPivotSize(const ImVec2* size) {
	ax::NodeEditor::PinPivotSize(*size);
}

void axNodeEditor_PinPivotScale(const ImVec2* scale) {
	ax::NodeEditor::PinPivotScale(*scale);
}

void axNodeEditor_PinPivotAlignment(const ImVec2* alignment) {
	ax::NodeEditor::PinPivotAlignment(*alignment);
}

void axNodeEditor_EndPin() {
	ax::NodeEditor::EndPin();
}

void axNodeEditor_Group(const ImVec2* size) {
	ax::NodeEditor::Group(*size);
}

void axNodeEditor_EndNode() {
	ax::NodeEditor::EndNode();
}

bool axNodeEditor_BeginGroupHint(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::BeginGroupHint(nodeId);
}

ImVec2 axNodeEditor_GetGroupMin() {
	return ax::NodeEditor::GetGroupMin();
}

ImVec2 axNodeEditor_GetGroupMax() {
	return ax::NodeEditor::GetGroupMax();
}

ImDrawList* axNodeEditor_GetHintForegroundDrawList() {
	return ax::NodeEditor::GetHintForegroundDrawList();
}

ImDrawList* axNodeEditor_GetHintBackgroundDrawList() {
	return ax::NodeEditor::GetHintBackgroundDrawList();
}

void axNodeEditor_EndGroupHint() {
	ax::NodeEditor::EndGroupHint();
}

ImDrawList* axNodeEditor_GetNodeBackgroundDrawList(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::GetNodeBackgroundDrawList(nodeId);
}

bool axNodeEditor_Link(axNodeEditor_LinkId id, axNodeEditor_PinId startPinId, axNodeEditor_PinId endPinId, const ImVec4* color, float thickness) {
	return ax::NodeEditor::Link(id, startPinId, endPinId, *color, thickness);
}

void axNodeEditor_Flow(axNodeEditor_LinkId linkId, axNodeEditor_FlowDirection direction) {
	ax::NodeEditor::Flow(linkId, direction);
}

bool axNodeEditor_BeginCreate(const ImVec4* color, float thickness) {
	return ax::NodeEditor::BeginCreate(*color, thickness);
}

bool axNodeEditor_QueryNewLink(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId) {
	return ax::NodeEditor::QueryNewLink(startId, endId);
}

bool axNodeEditor_QueryNewLink(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId, const ImVec4* color, float thickness) {
	return ax::NodeEditor::QueryNewLink(startId, endId, *color, thickness);
}

bool axNodeEditor_QueryNewNode(axNodeEditor_PinId* pinId) {
	return ax::NodeEditor::QueryNewNode(pinId);
}

bool axNodeEditor_QueryNewNode(axNodeEditor_PinId* pinId, const ImVec4* color, float thickness) {
	return ax::NodeEditor::QueryNewNode(pinId, *color, thickness);
}

bool axNodeEditor_AcceptNewItem() {
	return ax::NodeEditor::AcceptNewItem();
}

bool axNodeEditor_AcceptNewItem(const ImVec4* color, float thickness) {
	return ax::NodeEditor::AcceptNewItem(*color, thickness);
}

void axNodeEditor_RejectNewItem() {
	ax::NodeEditor::RejectNewItem();
}

void axNodeEditor_RejectNewItem(const ImVec4* color, float thickness) {
	ax::NodeEditor::RejectNewItem(*color, thickness);
}

void axNodeEditor_EndCreate() {
	ax::NodeEditor::EndCreate();
}

bool axNodeEditor_BeginDelete() {
	return ax::NodeEditor::BeginDelete();
}

bool axNodeEditor_QueryDeletedLink(axNodeEditor_LinkId* linkId, axNodeEditor_PinId* startId, axNodeEditor_PinId* endId) {
	return ax::NodeEditor::QueryDeletedLink(linkId, startId, endId);
}

bool axNodeEditor_QueryDeletedNode(axNodeEditor_NodeId* nodeId) {
	return ax::NodeEditor::QueryDeletedNode(nodeId);
}

bool axNodeEditor_AcceptDeletedItem(bool deleteDependencies) {
	return ax::NodeEditor::AcceptDeletedItem(deleteDependencies);
}

void axNodeEditor_RejectDeletedItem() {
	ax::NodeEditor::RejectDeletedItem();
}

void axNodeEditor_EndDelete() {
	ax::NodeEditor::EndDelete();
}

void axNodeEditor_SetNodePosition(axNodeEditor_NodeId nodeId, const ImVec2* editorPosition) {
	ax::NodeEditor::SetNodePosition(nodeId, *editorPosition);
}

void axNodeEditor_SetGroupSize(axNodeEditor_NodeId nodeId, const ImVec2* size) {
	ax::NodeEditor::SetGroupSize(nodeId, *size);
}

ImVec2 axNodeEditor_GetNodePosition(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::GetNodePosition(nodeId);
}

ImVec2 axNodeEditor_GetNodeSize(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::GetNodeSize(nodeId);
}

void axNodeEditor_CenterNodeOnScreen(axNodeEditor_NodeId nodeId) {
	ax::NodeEditor::CenterNodeOnScreen(nodeId);
}

void axNodeEditor_SetNodeZPosition(axNodeEditor_NodeId nodeId, float z) {
	ax::NodeEditor::SetNodeZPosition(nodeId, z);
}

float axNodeEditor_GetNodeZPosition(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::GetNodeZPosition(nodeId);
}

void axNodeEditor_RestoreNodeState(axNodeEditor_NodeId nodeId) {
	ax::NodeEditor::RestoreNodeState(nodeId);
}

void axNodeEditor_Suspend() {
	ax::NodeEditor::Suspend();
}

void axNodeEditor_Resume() {
	ax::NodeEditor::Resume();
}

bool axNodeEditor_IsSuspended() {
	return ax::NodeEditor::IsSuspended();
}

bool axNodeEditor_IsActive() {
	return ax::NodeEditor::IsActive();
}

bool axNodeEditor_HasSelectionChanged() {
	return ax::NodeEditor::HasSelectionChanged();
}

int axNodeEditor_GetSelectedObjectCount() {
	return ax::NodeEditor::GetSelectedObjectCount();
}

int axNodeEditor_GetSelectedNodes(axNodeEditor_NodeId* nodes, int size) {
	return ax::NodeEditor::GetSelectedNodes(nodes, size);
}

int axNodeEditor_GetSelectedLinks(axNodeEditor_LinkId* links, int size) {
	return ax::NodeEditor::GetSelectedLinks(links, size);
}

bool axNodeEditor_IsNodeSelected(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::IsNodeSelected(nodeId);
}

bool axNodeEditor_IsLinkSelected(axNodeEditor_LinkId linkId) {
	return ax::NodeEditor::IsLinkSelected(linkId);
}

void axNodeEditor_ClearSelection() {
	ax::NodeEditor::ClearSelection();
}

void axNodeEditor_SelectNode(axNodeEditor_NodeId nodeId, bool append) {
	ax::NodeEditor::SelectNode(nodeId, append);
}

void axNodeEditor_SelectLink(axNodeEditor_LinkId linkId, bool append) {
	ax::NodeEditor::SelectLink(linkId, append);
}

void axNodeEditor_DeselectNode(axNodeEditor_NodeId nodeId) {
	ax::NodeEditor::DeselectNode(nodeId);
}

void axNodeEditor_DeselectLink(axNodeEditor_LinkId linkId) {
	ax::NodeEditor::DeselectLink(linkId);
}

bool axNodeEditor_DeleteNode(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::DeleteNode(nodeId);
}

bool axNodeEditor_DeleteLink(axNodeEditor_LinkId linkId) {
	return ax::NodeEditor::DeleteLink(linkId);
}

bool axNodeEditor_HasAnyLinks(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::HasAnyLinks(nodeId);
}

bool axNodeEditor_HasAnyLinks(axNodeEditor_PinId pinId) {
	return ax::NodeEditor::HasAnyLinks(pinId);
}

int axNodeEditor_BreakLinks(axNodeEditor_NodeId nodeId) {
	return ax::NodeEditor::BreakLinks(nodeId);
}

int axNodeEditor_BreakLinks(axNodeEditor_PinId pinId) {
	return ax::NodeEditor::BreakLinks(pinId);
}

void axNodeEditor_NavigateToContent(float duration) {
	ax::NodeEditor::NavigateToContent(duration);
}

void axNodeEditor_NavigateToSelection(bool zoomIn, float duration) {
	ax::NodeEditor::NavigateToSelection(zoomIn, duration);
}

bool axNodeEditor_ShowNodeContextMenu(axNodeEditor_NodeId* nodeId) {
	return ax::NodeEditor::ShowNodeContextMenu(nodeId);
}

bool axNodeEditor_ShowPinContextMenu(axNodeEditor_PinId* pinId) {
	return ax::NodeEditor::ShowPinContextMenu(pinId);
}

bool axNodeEditor_ShowLinkContextMenu(axNodeEditor_LinkId* linkId) {
	return ax::NodeEditor::ShowLinkContextMenu(linkId);
}

bool axNodeEditor_ShowBackgroundContextMenu() {
	return ax::NodeEditor::ShowBackgroundContextMenu();
}

void axNodeEditor_EnableShortcuts(bool enable) {
	ax::NodeEditor::EnableShortcuts(enable);
}

bool axNodeEditor_AreShortcutsEnabled() {
	return ax::NodeEditor::AreShortcutsEnabled();
}

bool axNodeEditor_BeginShortcut() {
	return ax::NodeEditor::BeginShortcut();
}

bool axNodeEditor_AcceptCut() {
	return ax::NodeEditor::AcceptCut();
}

bool axNodeEditor_AcceptCopy() {
	return ax::NodeEditor::AcceptCopy();
}

bool axNodeEditor_AcceptPaste() {
	return ax::NodeEditor::AcceptPaste();
}

bool axNodeEditor_AcceptDuplicate() {
	return ax::NodeEditor::AcceptDuplicate();
}

bool axNodeEditor_AcceptCreateNode() {
	return ax::NodeEditor::AcceptCreateNode();
}

int axNodeEditor_GetActionContextSize() {
	return ax::NodeEditor::GetActionContextSize();
}

int axNodeEditor_GetActionContextNodes(axNodeEditor_NodeId* nodes, int size) {
	return ax::NodeEditor::GetActionContextNodes(nodes, size);
}

int axNodeEditor_GetActionContextLinks(axNodeEditor_LinkId* links, int size) {
	return ax::NodeEditor::GetActionContextLinks(links, size);
}

void axNodeEditor_EndShortcut() {
	ax::NodeEditor::EndShortcut();
}

float axNodeEditor_GetCurrentZoom() {
	return ax::NodeEditor::GetCurrentZoom();
}

axNodeEditor_NodeId axNodeEditor_GetHoveredNode() {
	return ax::NodeEditor::GetHoveredNode();
}

axNodeEditor_PinId axNodeEditor_GetHoveredPin() {
	return ax::NodeEditor::GetHoveredPin();
}

axNodeEditor_LinkId axNodeEditor_GetHoveredLink() {
	return ax::NodeEditor::GetHoveredLink();
}

axNodeEditor_NodeId axNodeEditor_GetDoubleClickedNode() {
	return ax::NodeEditor::GetDoubleClickedNode();
}

axNodeEditor_PinId axNodeEditor_GetDoubleClickedPin() {
	return ax::NodeEditor::GetDoubleClickedPin();
}

axNodeEditor_LinkId axNodeEditor_GetDoubleClickedLink() {
	return ax::NodeEditor::GetDoubleClickedLink();
}

bool axNodeEditor_IsBackgroundClicked() {
	return ax::NodeEditor::IsBackgroundClicked();
}

bool axNodeEditor_IsBackgroundDoubleClicked() {
	return ax::NodeEditor::IsBackgroundDoubleClicked();
}

int axNodeEditor_GetBackgroundClickButtonIndex() {
	return ax::NodeEditor::GetBackgroundClickButtonIndex();
}

int axNodeEditor_GetBackgroundDoubleClickButtonIndex() {
	return ax::NodeEditor::GetBackgroundDoubleClickButtonIndex();
}

bool axNodeEditor_GetLinkPins(axNodeEditor_LinkId linkId, axNodeEditor_PinId* startPinId, axNodeEditor_PinId* endPinId) {
	return ax::NodeEditor::GetLinkPins(linkId, startPinId, endPinId);
}

bool axNodeEditor_PinHadAnyLinks(axNodeEditor_PinId pinId) {
	return ax::NodeEditor::PinHadAnyLinks(pinId);
}

ImVec2 axNodeEditor_GetScreenSize() {
	return ax::NodeEditor::GetScreenSize();
}

ImVec2 axNodeEditor_ScreenToCanvas(const ImVec2* pos) {
	return ax::NodeEditor::ScreenToCanvas(*pos);
}

ImVec2 axNodeEditor_CanvasToScreen(const ImVec2* pos) {
	return ax::NodeEditor::CanvasToScreen(*pos);
}

int axNodeEditor_GetNodeCount() {
	return ax::NodeEditor::GetNodeCount();
}

int axNodeEditor_GetOrderedNodeIds(axNodeEditor_NodeId* nodes, int size) {
	return ax::NodeEditor::GetOrderedNodeIds(nodes, size);
}

axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __0) {
	return &__self->operator=(*__0);
}

unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __self) {
	return __self->Get();
}

axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __0) {
	return &__self->operator=(*__0);
}

unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __self) {
	return __self->Get();
}

axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __0) {
	return &__self->operator=(*__0);
}

unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __self) {
	return __self->Get();
}

axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __0) {
	return &__self->operator=(*__0);
}

axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __0) {
	return &__self->operator=(std::move(*__0));
}

axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __0) {
	return &__self->operator=(*__0);
}

axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __0) {
	return &__self->operator=(std::move(*__0));
}

axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* axNodeEditorDetails_SafePointerType_axNodeEditor_PinId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __0) {
	return &__self->operator=(*__0);
}

axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* axNodeEditorDetails_SafePointerType_axNodeEditor_PinId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __0) {
	return &__self->operator=(std::move(*__0));
}

