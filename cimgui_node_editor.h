#pragma once
#include "cimgui_node_editor_export.h"
#if !FOR_WRAPPER_IMPL

enum axNodeEditor_PinKind {
	Input = 0,
	Output = 1
};

enum axNodeEditor_FlowDirection {
	Forward = 0,
	Backward = 1
};

enum axNodeEditor_CanvasSizeMode {
	FitVerticalView = 0,
	FitHorizontalView = 1,
	CenterOnly = 2
};

enum axNodeEditor_SaveReasonFlags {
	None = 0,
	Navigation = 1,
	Position = 2,
	Size = 4,
	Selection = 8,
	AddNode = 16,
	RemoveNode = 32,
	User = 64
};

enum axNodeEditor_StyleColor {
	StyleColor_Bg = 0,
	StyleColor_Grid = 1,
	StyleColor_NodeBg = 2,
	StyleColor_NodeBorder = 3,
	StyleColor_HovNodeBorder = 4,
	StyleColor_SelNodeBorder = 5,
	StyleColor_NodeSelRect = 6,
	StyleColor_NodeSelRectBorder = 7,
	StyleColor_HovLinkBorder = 8,
	StyleColor_SelLinkBorder = 9,
	StyleColor_HighlightLinkBorder = 10,
	StyleColor_LinkSelRect = 11,
	StyleColor_LinkSelRectBorder = 12,
	StyleColor_PinRect = 13,
	StyleColor_PinRectBorder = 14,
	StyleColor_Flow = 15,
	StyleColor_FlowMarker = 16,
	StyleColor_GroupBg = 17,
	StyleColor_GroupBorder = 18,
	StyleColor_Count = 19
};

enum axNodeEditor_StyleVar {
	StyleVar_NodePadding = 0,
	StyleVar_NodeRounding = 1,
	StyleVar_NodeBorderWidth = 2,
	StyleVar_HoveredNodeBorderWidth = 3,
	StyleVar_SelectedNodeBorderWidth = 4,
	StyleVar_PinRounding = 5,
	StyleVar_PinBorderWidth = 6,
	StyleVar_LinkStrength = 7,
	StyleVar_SourceDirection = 8,
	StyleVar_TargetDirection = 9,
	StyleVar_ScrollDuration = 10,
	StyleVar_FlowMarkerDistance = 11,
	StyleVar_FlowSpeed = 12,
	StyleVar_FlowDuration = 13,
	StyleVar_PivotAlignment = 14,
	StyleVar_PivotSize = 15,
	StyleVar_PivotScale = 16,
	StyleVar_PinCorners = 17,
	StyleVar_PinRadius = 18,
	StyleVar_PinArrowSize = 19,
	StyleVar_PinArrowWidth = 20,
	StyleVar_GroupRounding = 21,
	StyleVar_GroupBorderWidth = 22,
	StyleVar_HighlightConnectedLinks = 23,
	StyleVar_SnapLinkToPinDir = 24,
	StyleVar_HoveredNodeBorderOffset = 25,
	StyleVar_SelectedNodeBorderOffset = 26,
	StyleVar_Count = 27
};

enum axDrawing_IconType {
	Flow = 0,
	Circle = 1,
	Square = 2,
	Grid = 3,
	RoundSquare = 4,
	Diamond = 5
};

struct axNodeEditor_Config;
typedef struct axNodeEditor_Config axNodeEditor_Config;
struct axNodeEditor_Style;
typedef struct axNodeEditor_Style axNodeEditor_Style;
struct axNodeEditor_NodeId;
typedef struct axNodeEditor_NodeId axNodeEditor_NodeId;
struct axNodeEditor_LinkId;
typedef struct axNodeEditor_LinkId axNodeEditor_LinkId;
struct axNodeEditor_PinId;
typedef struct axNodeEditor_PinId axNodeEditor_PinId;
struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId;
typedef struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId;
struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId;
typedef struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId;
struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId;
typedef struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId;
struct axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId;
typedef struct axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId;
struct axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId;
typedef struct axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId;
struct axNodeEditorDetails_SafePointerType_axNodeEditor_PinId;
typedef struct axNodeEditorDetails_SafePointerType_axNodeEditor_PinId axNodeEditorDetails_SafePointerType_axNodeEditor_PinId;
struct axNodeEditor_EditorContext;
typedef struct axNodeEditor_EditorContext axNodeEditor_EditorContext;

struct axNodeEditor_Config {
	const char*          SettingsFile;
	void (*)(void*)      BeginSaveSession;
	void (*)(void*)      EndSaveSession;
	bool (*)(const char*,void*,axNodeEditor_SaveReasonFlags,void*) SaveSettings;
	void* (*)(char*,void*) LoadSettings;
	bool (*)(axNodeEditor_NodeId,const char*,void*,axNodeEditor_SaveReasonFlags,void*) SaveNodeSettings;
	void* (*)(axNodeEditor_NodeId,char*,void*) LoadNodeSettings;
	void*                UserPointer;
	ImVector_float       CustomZoomLevels;
	axNodeEditor_CanvasSizeMode CanvasSizeMode;
	int                  DragButtonIndex;
	int                  SelectButtonIndex;
	int                  NavigateButtonIndex;
	int                  ContextMenuButtonIndex;
	bool                 EnableSmoothZoom;
	float                SmoothZoomPower;
};

struct axNodeEditor_Style {
	ImVec4               NodePadding;
	float                NodeRounding;
	float                NodeBorderWidth;
	float                HoveredNodeBorderWidth;
	float                HoverNodeBorderOffset;
	float                SelectedNodeBorderWidth;
	float                SelectedNodeBorderOffset;
	float                PinRounding;
	float                PinBorderWidth;
	float                LinkStrength;
	ImVec2               SourceDirection;
	ImVec2               TargetDirection;
	float                ScrollDuration;
	float                FlowMarkerDistance;
	float                FlowSpeed;
	float                FlowDuration;
	ImVec2               PivotAlignment;
	ImVec2               PivotSize;
	ImVec2               PivotScale;
	float                PinCorners;
	float                PinRadius;
	float                PinArrowSize;
	float                PinArrowWidth;
	float                GroupRounding;
	float                GroupBorderWidth;
	float                HighlightConnectedLinks;
	float                SnapLinkToPinDir;
	ImVec4[19]           Colors;
};

struct axNodeEditor_NodeId {
	axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId _base_axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId

};

struct axNodeEditor_LinkId {
	axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId _base_axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId

};

struct axNodeEditor_PinId {
	axNodeEditorDetails_SafePointerType_axNodeEditor_PinId _base_axNodeEditorDetails_SafePointerType_axNodeEditor_PinId

};

struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId {
	void*                m_Value;
};

struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId {
	void*                m_Value;
};

struct axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId {
	void*                m_Value;
};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId {
	axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId _base_axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId

};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId {
	axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId _base_axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId

};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_PinId {
	axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId _base_axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId

};

#else
typedef ax::NodeEditor::PinKind axNodeEditor_PinKind;
typedef ax::NodeEditor::FlowDirection axNodeEditor_FlowDirection;
typedef ax::NodeEditor::CanvasSizeMode axNodeEditor_CanvasSizeMode;
typedef ax::NodeEditor::SaveReasonFlags axNodeEditor_SaveReasonFlags;
typedef ax::NodeEditor::StyleColor axNodeEditor_StyleColor;
typedef ax::NodeEditor::StyleVar axNodeEditor_StyleVar;
typedef ax::Drawing::IconType axDrawing_IconType;
typedef ax::NodeEditor::Config axNodeEditor_Config;
typedef ax::NodeEditor::Style axNodeEditor_Style;
typedef ax::NodeEditor::NodeId axNodeEditor_NodeId;
typedef ax::NodeEditor::LinkId axNodeEditor_LinkId;
typedef ax::NodeEditor::PinId axNodeEditor_PinId;
typedef ax::NodeEditor::Details::SafeType<uintptr_t, ax::NodeEditor::NodeId> axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId;
typedef ax::NodeEditor::Details::SafeType<uintptr_t, ax::NodeEditor::LinkId> axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId;
typedef ax::NodeEditor::Details::SafeType<uintptr_t, ax::NodeEditor::PinId> axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::NodeId> axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::LinkId> axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::PinId> axNodeEditorDetails_SafePointerType_axNodeEditor_PinId;
typedef ax::NodeEditor::EditorContext axNodeEditor_EditorContext;
#endif // !FOR_WRAPPER_IMPL

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_SaveReasonFlags axNodeEditor_operator_pipe(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_SaveReasonFlags axNodeEditor_operator_amp(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SetCurrentEditor(axNodeEditor_EditorContext* ctx);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_EditorContext* axNodeEditor_GetCurrentEditor();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_EditorContext* axNodeEditor_CreateEditor(const axNodeEditor_Config* config);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_DestroyEditor(axNodeEditor_EditorContext* ctx);
CIMGUI_NODE_EDITOR_EXPORT const axNodeEditor_Config* axNodeEditor_GetConfig(axNodeEditor_EditorContext* ctx);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_Style* axNodeEditor_GetStyle();
CIMGUI_NODE_EDITOR_EXPORT const char* axNodeEditor_GetStyleColorName(axNodeEditor_StyleColor colorIndex);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PushStyleColor(axNodeEditor_StyleColor colorIndex, const ImVec4* color);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PopStyleColor(int count);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, float value);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PushStyleVar1(axNodeEditor_StyleVar varIndex, const ImVec2* value);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PushStyleVar2(axNodeEditor_StyleVar varIndex, const ImVec4* value);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PopStyleVar(int count);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_Begin(const char* id, const ImVec2* size);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_End();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_BeginNode(axNodeEditor_NodeId id);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_BeginPin(axNodeEditor_PinId id, axNodeEditor_PinKind kind);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PinRect(const ImVec2* a, const ImVec2* b);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PinPivotRect(const ImVec2* a, const ImVec2* b);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PinPivotSize(const ImVec2* size);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PinPivotScale(const ImVec2* scale);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_PinPivotAlignment(const ImVec2* alignment);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndPin();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_Group(const ImVec2* size);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndNode();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_BeginGroupHint(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_GetGroupMin();
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_GetGroupMax();
CIMGUI_NODE_EDITOR_EXPORT ImDrawList* axNodeEditor_GetHintForegroundDrawList();
CIMGUI_NODE_EDITOR_EXPORT ImDrawList* axNodeEditor_GetHintBackgroundDrawList();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndGroupHint();
CIMGUI_NODE_EDITOR_EXPORT ImDrawList* axNodeEditor_GetNodeBackgroundDrawList(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_Link(axNodeEditor_LinkId id, axNodeEditor_PinId startPinId, axNodeEditor_PinId endPinId, const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_Flow(axNodeEditor_LinkId linkId, axNodeEditor_FlowDirection direction);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_BeginCreate(const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryNewLink(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryNewLink1(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId, const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryNewNode(axNodeEditor_PinId* pinId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryNewNode1(axNodeEditor_PinId* pinId, const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptNewItem();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptNewItem1(const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_RejectNewItem();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_RejectNewItem1(const ImVec4* color, float thickness);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndCreate();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_BeginDelete();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryDeletedLink(axNodeEditor_LinkId* linkId, axNodeEditor_PinId* startId, axNodeEditor_PinId* endId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_QueryDeletedNode(axNodeEditor_NodeId* nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptDeletedItem(bool deleteDependencies);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_RejectDeletedItem();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndDelete();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SetNodePosition(axNodeEditor_NodeId nodeId, const ImVec2* editorPosition);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SetGroupSize(axNodeEditor_NodeId nodeId, const ImVec2* size);
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_GetNodePosition(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_GetNodeSize(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_CenterNodeOnScreen(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SetNodeZPosition(axNodeEditor_NodeId nodeId, float z);
CIMGUI_NODE_EDITOR_EXPORT float axNodeEditor_GetNodeZPosition(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_RestoreNodeState(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_Suspend();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_Resume();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsSuspended();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsActive();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_HasSelectionChanged();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetSelectedObjectCount();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetSelectedNodes(axNodeEditor_NodeId* nodes, int size);
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetSelectedLinks(axNodeEditor_LinkId* links, int size);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsNodeSelected(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsLinkSelected(axNodeEditor_LinkId linkId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_ClearSelection();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SelectNode(axNodeEditor_NodeId nodeId, bool append);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_SelectLink(axNodeEditor_LinkId linkId, bool append);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_DeselectNode(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_DeselectLink(axNodeEditor_LinkId linkId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_DeleteNode(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_DeleteLink(axNodeEditor_LinkId linkId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_HasAnyLinks(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_HasAnyLinks1(axNodeEditor_PinId pinId);
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_BreakLinks(axNodeEditor_NodeId nodeId);
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_BreakLinks1(axNodeEditor_PinId pinId);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_NavigateToContent(float duration);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_NavigateToSelection(bool zoomIn, float duration);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_ShowNodeContextMenu(axNodeEditor_NodeId* nodeId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_ShowPinContextMenu(axNodeEditor_PinId* pinId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_ShowLinkContextMenu(axNodeEditor_LinkId* linkId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_ShowBackgroundContextMenu();
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EnableShortcuts(bool enable);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AreShortcutsEnabled();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_BeginShortcut();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptCut();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptCopy();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptPaste();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptDuplicate();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_AcceptCreateNode();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetActionContextSize();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetActionContextNodes(axNodeEditor_NodeId* nodes, int size);
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetActionContextLinks(axNodeEditor_LinkId* links, int size);
CIMGUI_NODE_EDITOR_EXPORT void axNodeEditor_EndShortcut();
CIMGUI_NODE_EDITOR_EXPORT float axNodeEditor_GetCurrentZoom();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_NodeId axNodeEditor_GetHoveredNode();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_PinId axNodeEditor_GetHoveredPin();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_LinkId axNodeEditor_GetHoveredLink();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_NodeId axNodeEditor_GetDoubleClickedNode();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_PinId axNodeEditor_GetDoubleClickedPin();
CIMGUI_NODE_EDITOR_EXPORT axNodeEditor_LinkId axNodeEditor_GetDoubleClickedLink();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsBackgroundClicked();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_IsBackgroundDoubleClicked();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetBackgroundClickButtonIndex();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetBackgroundDoubleClickButtonIndex();
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_GetLinkPins(axNodeEditor_LinkId linkId, axNodeEditor_PinId* startPinId, axNodeEditor_PinId* endPinId);
CIMGUI_NODE_EDITOR_EXPORT bool axNodeEditor_PinHadAnyLinks(axNodeEditor_PinId pinId);
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_GetScreenSize();
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_ScreenToCanvas(const ImVec2* pos);
CIMGUI_NODE_EDITOR_EXPORT ImVec2 axNodeEditor_CanvasToScreen(const ImVec2* pos);
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetNodeCount();
CIMGUI_NODE_EDITOR_EXPORT int axNodeEditor_GetOrderedNodeIds(axNodeEditor_NodeId* nodes, int size);
CIMGUI_NODE_EDITOR_EXPORT void axDrawing_DrawIcon(ImDrawList* drawList, const ImVec2* a, const ImVec2* b, axDrawing_IconType type, bool filled, unsigned int color, unsigned int innerColor);
CIMGUI_NODE_EDITOR_EXPORT void axWidgets_Icon(const ImVec2* size, axDrawing_IconType type, bool filled, const ImVec4* color, const ImVec4* innerColor);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId_operator_equal(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId* __self, const axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId* __0);
CIMGUI_NODE_EDITOR_EXPORT void* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId_Get(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_NodeId* __self);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId_operator_equal(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId* __self, const axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId* __0);
CIMGUI_NODE_EDITOR_EXPORT void* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId_Get(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_LinkId* __self);
CIMGUI_NODE_EDITOR_EXPORT axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId_operator_equal(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId* __self, const axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId* __0);
CIMGUI_NODE_EDITOR_EXPORT void* axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId_Get(axNodeEditorDetails_SafeType_voidptr_axNodeEditor_PinId* __self);
#ifdef __cplusplus
} // extern "C"
#endif // __cplusplus
