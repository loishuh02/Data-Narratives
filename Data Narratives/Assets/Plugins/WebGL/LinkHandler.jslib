mergeInto(LibraryManager.library, {
  OpenWindow: function (link) {
    var url = UTF8ToString(link);
    window.open(url, "_blank");
  },
});