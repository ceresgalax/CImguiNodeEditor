#pragma once
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

struct axNodeEditor_Config;
typedef struct axNodeEditor_Config axNodeEditor_Config;
struct axNodeEditor_Style;
typedef struct axNodeEditor_Style axNodeEditor_Style;
struct axNodeEditorDetails_SafeType;
typedef struct axNodeEditorDetails_SafeType axNodeEditorDetails_SafeType;
struct axNodeEditorDetails_SafeType;
typedef struct axNodeEditorDetails_SafeType axNodeEditorDetails_SafeType;
struct axNodeEditorDetails_SafeType;
typedef struct axNodeEditorDetails_SafeType axNodeEditorDetails_SafeType;
struct axNodeEditorDetails_SafePointerType;
typedef struct axNodeEditorDetails_SafePointerType axNodeEditorDetails_SafePointerType;
struct axNodeEditorDetails_SafePointerType;
typedef struct axNodeEditorDetails_SafePointerType axNodeEditorDetails_SafePointerType;
struct axNodeEditorDetails_SafePointerType;
typedef struct axNodeEditorDetails_SafePointerType axNodeEditorDetails_SafePointerType;
struct axNodeEditor_NodeId;
typedef struct axNodeEditor_NodeId axNodeEditor_NodeId;
struct axNodeEditor_LinkId;
typedef struct axNodeEditor_LinkId axNodeEditor_LinkId;
struct axNodeEditor_PinId;
typedef struct axNodeEditor_PinId axNodeEditor_PinId;
struct axNodeEditor_EditorContext;
typedef struct axNodeEditor_EditorContext axNodeEditor_EditorContext;

struct axNodeEditor_Config {
	const char*          SettingsFile;
	void (*)(void*)      BeginSaveSession;
	void (*)(void*)      EndSaveSession;
	bool (*)(const char*,unsigned long long,axNodeEditor_SaveReasonFlags,void*) SaveSettings;
	unsigned long long (*)(char*,void*) LoadSettings;
	bool (*)(axNodeEditor_NodeId,const char*,unsigned long long,axNodeEditor_SaveReasonFlags,void*) SaveNodeSettings;
	unsigned long long (*)(axNodeEditor_NodeId,char*,void*) LoadNodeSettings;
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

struct axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId {
	unsigned long long   m_Value;
};

struct axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId {
	unsigned long long   m_Value;
};

struct axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId {
	unsigned long long   m_Value;
};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId {
	axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId _base_axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId

};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId {
	axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId _base_axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId

};

struct axNodeEditorDetails_SafePointerType_axNodeEditor_PinId {
	axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId _base_axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId

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

#else
typedef ax::NodeEditor::PinKind axNodeEditor_PinKind;
typedef ax::NodeEditor::FlowDirection axNodeEditor_FlowDirection;
typedef ax::NodeEditor::CanvasSizeMode axNodeEditor_CanvasSizeMode;
typedef ax::NodeEditor::SaveReasonFlags axNodeEditor_SaveReasonFlags;
typedef ax::NodeEditor::StyleColor axNodeEditor_StyleColor;
typedef ax::NodeEditor::StyleVar axNodeEditor_StyleVar;
typedef ax::NodeEditor::Config axNodeEditor_Config;
typedef ax::NodeEditor::Style axNodeEditor_Style;
typedef ax::NodeEditor::Details::SafeType<unsigned long long, ax::NodeEditor::NodeId> axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId;
typedef ax::NodeEditor::Details::SafeType<unsigned long long, ax::NodeEditor::LinkId> axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId;
typedef ax::NodeEditor::Details::SafeType<unsigned long long, ax::NodeEditor::PinId> axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::NodeId> axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::LinkId> axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId;
typedef ax::NodeEditor::Details::SafePointerType<ax::NodeEditor::PinId> axNodeEditorDetails_SafePointerType_axNodeEditor_PinId;
typedef ax::NodeEditor::NodeId axNodeEditor_NodeId;
typedef ax::NodeEditor::LinkId axNodeEditor_LinkId;
typedef ax::NodeEditor::PinId axNodeEditor_PinId;
typedef ax::NodeEditor::EditorContext axNodeEditor_EditorContext;
#endif // !FOR_WRAPPER_IMPL

axNodeEditor_SaveReasonFlags axNodeEditor_operator_pipe(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs);
axNodeEditor_SaveReasonFlags axNodeEditor_operator_amp(axNodeEditor_SaveReasonFlags lhs, axNodeEditor_SaveReasonFlags rhs);
void axNodeEditor_SetCurrentEditor(axNodeEditor_EditorContext* ctx);
axNodeEditor_EditorContext* axNodeEditor_GetCurrentEditor();
axNodeEditor_EditorContext* axNodeEditor_CreateEditor(const axNodeEditor_Config* config);
void axNodeEditor_DestroyEditor(axNodeEditor_EditorContext* ctx);
const axNodeEditor_Config* axNodeEditor_GetConfig(axNodeEditor_EditorContext* ctx);
axNodeEditor_Style* axNodeEditor_GetStyle();
const char* axNodeEditor_GetStyleColorName(axNodeEditor_StyleColor colorIndex);
void axNodeEditor_PushStyleColor(axNodeEditor_StyleColor colorIndex, const ImVec4* color);
void axNodeEditor_PopStyleColor(int count);
void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, float value);
void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, const ImVec2* value);
void axNodeEditor_PushStyleVar(axNodeEditor_StyleVar varIndex, const ImVec4* value);
void axNodeEditor_PopStyleVar(int count);
void axNodeEditor_Begin(const char* id, const ImVec2* size);
void axNodeEditor_End();
void axNodeEditor_BeginNode(axNodeEditor_NodeId id);
void axNodeEditor_BeginPin(axNodeEditor_PinId id, axNodeEditor_PinKind kind);
void axNodeEditor_PinRect(const ImVec2* a, const ImVec2* b);
void axNodeEditor_PinPivotRect(const ImVec2* a, const ImVec2* b);
void axNodeEditor_PinPivotSize(const ImVec2* size);
void axNodeEditor_PinPivotScale(const ImVec2* scale);
void axNodeEditor_PinPivotAlignment(const ImVec2* alignment);
void axNodeEditor_EndPin();
void axNodeEditor_Group(const ImVec2* size);
void axNodeEditor_EndNode();
bool axNodeEditor_BeginGroupHint(axNodeEditor_NodeId nodeId);
ImVec2 axNodeEditor_GetGroupMin();
ImVec2 axNodeEditor_GetGroupMax();
ImDrawList* axNodeEditor_GetHintForegroundDrawList();
ImDrawList* axNodeEditor_GetHintBackgroundDrawList();
void axNodeEditor_EndGroupHint();
ImDrawList* axNodeEditor_GetNodeBackgroundDrawList(axNodeEditor_NodeId nodeId);
bool axNodeEditor_Link(axNodeEditor_LinkId id, axNodeEditor_PinId startPinId, axNodeEditor_PinId endPinId, const ImVec4* color, float thickness);
void axNodeEditor_Flow(axNodeEditor_LinkId linkId, axNodeEditor_FlowDirection direction);
bool axNodeEditor_BeginCreate(const ImVec4* color, float thickness);
bool axNodeEditor_QueryNewLink(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId);
bool axNodeEditor_QueryNewLink(axNodeEditor_PinId* startId, axNodeEditor_PinId* endId, const ImVec4* color, float thickness);
bool axNodeEditor_QueryNewNode(axNodeEditor_PinId* pinId);
bool axNodeEditor_QueryNewNode(axNodeEditor_PinId* pinId, const ImVec4* color, float thickness);
bool axNodeEditor_AcceptNewItem();
bool axNodeEditor_AcceptNewItem(const ImVec4* color, float thickness);
void axNodeEditor_RejectNewItem();
void axNodeEditor_RejectNewItem(const ImVec4* color, float thickness);
void axNodeEditor_EndCreate();
bool axNodeEditor_BeginDelete();
bool axNodeEditor_QueryDeletedLink(axNodeEditor_LinkId* linkId, axNodeEditor_PinId* startId, axNodeEditor_PinId* endId);
bool axNodeEditor_QueryDeletedNode(axNodeEditor_NodeId* nodeId);
bool axNodeEditor_AcceptDeletedItem(bool deleteDependencies);
void axNodeEditor_RejectDeletedItem();
void axNodeEditor_EndDelete();
void axNodeEditor_SetNodePosition(axNodeEditor_NodeId nodeId, const ImVec2* editorPosition);
void axNodeEditor_SetGroupSize(axNodeEditor_NodeId nodeId, const ImVec2* size);
ImVec2 axNodeEditor_GetNodePosition(axNodeEditor_NodeId nodeId);
ImVec2 axNodeEditor_GetNodeSize(axNodeEditor_NodeId nodeId);
void axNodeEditor_CenterNodeOnScreen(axNodeEditor_NodeId nodeId);
void axNodeEditor_SetNodeZPosition(axNodeEditor_NodeId nodeId, float z);
float axNodeEditor_GetNodeZPosition(axNodeEditor_NodeId nodeId);
void axNodeEditor_RestoreNodeState(axNodeEditor_NodeId nodeId);
void axNodeEditor_Suspend();
void axNodeEditor_Resume();
bool axNodeEditor_IsSuspended();
bool axNodeEditor_IsActive();
bool axNodeEditor_HasSelectionChanged();
int axNodeEditor_GetSelectedObjectCount();
int axNodeEditor_GetSelectedNodes(axNodeEditor_NodeId* nodes, int size);
int axNodeEditor_GetSelectedLinks(axNodeEditor_LinkId* links, int size);
bool axNodeEditor_IsNodeSelected(axNodeEditor_NodeId nodeId);
bool axNodeEditor_IsLinkSelected(axNodeEditor_LinkId linkId);
void axNodeEditor_ClearSelection();
void axNodeEditor_SelectNode(axNodeEditor_NodeId nodeId, bool append);
void axNodeEditor_SelectLink(axNodeEditor_LinkId linkId, bool append);
void axNodeEditor_DeselectNode(axNodeEditor_NodeId nodeId);
void axNodeEditor_DeselectLink(axNodeEditor_LinkId linkId);
bool axNodeEditor_DeleteNode(axNodeEditor_NodeId nodeId);
bool axNodeEditor_DeleteLink(axNodeEditor_LinkId linkId);
bool axNodeEditor_HasAnyLinks(axNodeEditor_NodeId nodeId);
bool axNodeEditor_HasAnyLinks(axNodeEditor_PinId pinId);
int axNodeEditor_BreakLinks(axNodeEditor_NodeId nodeId);
int axNodeEditor_BreakLinks(axNodeEditor_PinId pinId);
void axNodeEditor_NavigateToContent(float duration);
void axNodeEditor_NavigateToSelection(bool zoomIn, float duration);
bool axNodeEditor_ShowNodeContextMenu(axNodeEditor_NodeId* nodeId);
bool axNodeEditor_ShowPinContextMenu(axNodeEditor_PinId* pinId);
bool axNodeEditor_ShowLinkContextMenu(axNodeEditor_LinkId* linkId);
bool axNodeEditor_ShowBackgroundContextMenu();
void axNodeEditor_EnableShortcuts(bool enable);
bool axNodeEditor_AreShortcutsEnabled();
bool axNodeEditor_BeginShortcut();
bool axNodeEditor_AcceptCut();
bool axNodeEditor_AcceptCopy();
bool axNodeEditor_AcceptPaste();
bool axNodeEditor_AcceptDuplicate();
bool axNodeEditor_AcceptCreateNode();
int axNodeEditor_GetActionContextSize();
int axNodeEditor_GetActionContextNodes(axNodeEditor_NodeId* nodes, int size);
int axNodeEditor_GetActionContextLinks(axNodeEditor_LinkId* links, int size);
void axNodeEditor_EndShortcut();
float axNodeEditor_GetCurrentZoom();
axNodeEditor_NodeId axNodeEditor_GetHoveredNode();
axNodeEditor_PinId axNodeEditor_GetHoveredPin();
axNodeEditor_LinkId axNodeEditor_GetHoveredLink();
axNodeEditor_NodeId axNodeEditor_GetDoubleClickedNode();
axNodeEditor_PinId axNodeEditor_GetDoubleClickedPin();
axNodeEditor_LinkId axNodeEditor_GetDoubleClickedLink();
bool axNodeEditor_IsBackgroundClicked();
bool axNodeEditor_IsBackgroundDoubleClicked();
int axNodeEditor_GetBackgroundClickButtonIndex();
int axNodeEditor_GetBackgroundDoubleClickButtonIndex();
bool axNodeEditor_GetLinkPins(axNodeEditor_LinkId linkId, axNodeEditor_PinId* startPinId, axNodeEditor_PinId* endPinId);
bool axNodeEditor_PinHadAnyLinks(axNodeEditor_PinId pinId);
ImVec2 axNodeEditor_GetScreenSize();
ImVec2 axNodeEditor_ScreenToCanvas(const ImVec2* pos);
ImVec2 axNodeEditor_CanvasToScreen(const ImVec2* pos);
int axNodeEditor_GetNodeCount();
int axNodeEditor_GetOrderedNodeIds(axNodeEditor_NodeId* nodes, int size);
axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __0);
unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_NodeId* __self);
axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __0);
unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_LinkId* __self);
axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId_operator_equal(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __self, axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __0);
unsigned long long axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId_Get(axNodeEditorDetails_SafeType_unsignedlonglong_axNodeEditor_PinId* __self);
axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __0);
axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_NodeId* __0);
axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __0);
axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_LinkId* __0);
axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* axNodeEditorDetails_SafePointerType_axNodeEditor_PinId_operator_equal(axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __0);
axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* axNodeEditorDetails_SafePointerType_axNodeEditor_PinId_operator_equal1(axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __self, axNodeEditorDetails_SafePointerType_axNodeEditor_PinId* __0);
