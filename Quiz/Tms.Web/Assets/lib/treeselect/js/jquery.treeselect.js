(function($) {

  // From https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/keys
  if (!Object.keys) {
    Object.keys = (function () {
      'use strict';
      var hasOwnProperty = Object.prototype.hasOwnProperty,
        hasDontEnumBug = !({toString: null}).propertyIsEnumerable('toString'),
        dontEnums = [
          'toString',
          'toLocaleString',
          'valueOf',
          'hasOwnProperty',
          'isPrototypeOf',
          'propertyIsEnumerable',
          'constructor'
        ],
        dontEnumsLength = dontEnums.length;

      return function (obj) {
        if (typeof obj !== 'object' && (typeof obj !== 'function' || obj === null)) {
          throw new TypeError('Object.keys called on non-object');
        }

        var result = [], prop, i;

        for (prop in obj) {
          if (hasOwnProperty.call(obj, prop)) {
            result.push(prop);
          }
        }

        if (hasDontEnumBug) {
          for (i = 0; i < dontEnumsLength; i++) {
            if (hasOwnProperty.call(obj, dontEnums[i])) {
              result.push(dontEnums[i]);
            }
          }
        }
        return result;
      };
    }());
  }

  // The tree select control.
  $.fn.treeselect = function(params) {

    // Setup the default parameters for the tree select control.
    params = $.extend({
      colwidth: 18,               /** The width of the columns. */
      default_value: {},          /** An array of default values. */
      selected: null,             /** Callback when an item is selected. */
      treeloaded: null,           /** Called when the tree is loaded. */
      load: null,                 /** Callback to load new tree's */
      searcher: null,             /** Callback to search a tree */
      deepLoad: false,            /** Performs a deep load */
      onbuild: null,              /** Called when each node is building. */
      postbuild: null,            /** Called when the node is done building. */
      inputName: 'treeselect',    /** The input name. */
      autoSelectChildren: true,   /** Select chldrn when parent is selected. */
      showRoot: false,            /** Show the root item with a checkbox. */
      selectAll: false,           /** If we wish to see a select all. */
      selectAllText: 'Select All' /** The select all text. */
    }, params);

    /** Keep track of all loaded nodes */
    var loadedNodes = {};

    /** Variable for the busy states. */
    var busyloading = 'treebusy-loading';
    var busyloadingall = 'treebusy-loading-all';
    var busyselecting = 'treebusy-selecting';

    /**
     * Constructor.
     */
    var TreeNode = function(nodeparams, root) {

      // Determine if this is a root item.
      this.root = !!root;

      // Setup the parameters.
      nodeparams.title = nodeparams.title || 'anonymous';
      $.extend(this, {
        id: 0,                /** The ID of this node. */
        nodeloaded: false,    /** Flag to see if this node is loaded. */
        allLoaded: false,     /** Flag to see if we have loaded all nodes. */
        value: 0,             /** The input value for this node. */
        title: '',            /** The title of this node. */
        url: '',              /** The URL to this node. */
        has_children: true,   /** Boolean if this node has children. */
        children: [],         /** Array of children. */
        data: {},             /** Additional data to attach to the node. */
        level: 0,             /** The level of this node. */
        odd: false,           /** The odd/even state of this row. */
        checked: false,       /** If this node is checked. */
        busy: false,          /** If this node is busy. */
        display: $(),         /** The display of this node. */
        input: $(),           /** The input display. */
        link: $(),            /** The link display. */
        span: $(),            /** The span display. */
        childlist: $(),       /** The childlist display. */
        exclude: {}           /** An array of nodes to exclude for selection. */
      }, nodeparams);

      // Say that we are a TreeNode.
      this.isTreeNode = true;

      // Determine if a node is loading.
      this.loading = false;

      // The load callback queue.
      this.loadqueue = [];
    };

    /**
     * Set the busy cursor for this node.
     */
    TreeNode.prototype.setBusy = function(state, type) {

      // Make sure the state has changed.
      if (state != this.span.hasClass(type)) {
        this.busy = state;
        if (state) {

          // Set the busy type and treebusy.
          this.span.addClass(type);
          this.span.addClass('treebusy');
        }
        else {

          // Remove the busy type.
          this.span.removeClass(type);

          // Only remove the busy if the busy flags are empty.
          var othertype = (type == busyloading) ? busyselecting : busyloading;
          if (!this.span.hasClass(othertype)) {
            this.span.removeClass('treebusy');
          }
        }

      }
    };

    /**
     * Determines if this node is already loaded.
     */
    TreeNode.prototype.isLoaded = function() {
      var loaded = this.nodeloaded;
      loaded |= loadedNodes.hasOwnProperty(this.id);
      loaded |= !this.has_children;
      loaded |= (this.has_children && this.children.length > 0);
      return loaded;
    };

    /**
     * Loads the current node.
     *
     * @param {function} callback - The callback when the node is loaded.
     */
    TreeNode.prototype.loadNode = function(callback, hideBusy) {

      // If we are loading, then just add this callback to the queue and return.
      if (this.loading) {
        if (callback) {
          this.loadqueue.push(callback);
        }
        return;
      }

      // Trigger the callback when the node is done loading.
      var triggerCallback = function() {

        // Callback that we are loaded.
        if (callback) {
          callback(this);
        }

        // Process the loadqueue.
        for (var i in this.loadqueue) {
          this.loadqueue[i](this);
        }

        // Empty the loadqueue.
        this.loadqueue.length = 0;

        // Say we are not busy.
        if (!hideBusy) {
          this.setBusy(false, busyloading);
        }
      };

      // Say we are loading.
      this.loading = true;

      // Only load if we have not loaded yet.
      if (params.load && !this.isLoaded()) {

        // Make this node busy.
        if (!hideBusy) {
          this.setBusy(true, busyloading);
        }

        // Call the load function.
        params.load(this, (function(treenode) {
          return function(node) {

            // Only perform the merging and build if it hasn't loaded.
            if (!treenode.nodeloaded) {

              // Merge the result with this node.
              treenode = jQuery.extend(treenode, node);

              // Say this node is loaded.
              treenode.nodeloaded = true;

              // Add to the loaded nodes array.
              loadedNodes[treenode.id] = treenode.id;

              // Build the node.
              treenode.build(function() {

                // Callback that we are loaded.
                triggerCallback.call(treenode);
              });
            }
            else {

              // Callback that we are loaded.
              triggerCallback.call(treenode);
            }
          };
        })(this));
      }
      else if (callback) {

        // Just callback since we are already loaded.
        triggerCallback.call(this);
      }

      // Say that we are not loading anymore.
      this.loading = false;
    };

    /**
     * Recursively loads and builds all nodes beneath this node.
     *
     * @param {function} callback Called when the tree has loaded.
     * @param {function} operation Allow someone to perform an operation.
     */
    TreeNode.prototype.loadAll = function(callback, operation, hideBusy, ids) {
      ids = ids || {};

      // Make sure we are loaded first.
      this.loadNode(function(node) {

        // See if an operation needs to be performed.
        if (operation) {
          operation(node);
        }

        // Get our children count.
        var i = node.children.length, count = i;

        // If no children, then just call the callback immediately.
        if (!i || ids.hasOwnProperty(node.id)) {
          if (callback) {
            callback.call(node, node);
          }
          return;
        }

        // Add this to the ids to protect against recursion.
        ids[node.id] = node.id;

        // Make this node busy.
        if (!hideBusy) {
          node.setBusy(true, busyloadingall);
        }

        // Load children at a specific index.
        var loadChildren = function(index) {
          return function() {

            // Load this childs children...
            node.children[index].loadAll(function() {

              // Decrement the child count.
              count--;

              // If all children are done loading, call the callback.
              if (!count) {

                // Callback that we are done loading this tree.
                if (callback) {
                  callback.call(node, node);
                }

                // Make this node busy.
                if (!hideBusy) {
                  node.setBusy(false, busyloadingall);
                }
              }
            }, operation, hideBusy, ids);
          };
        };

        // Iterate through each child.
        while (i--) {

          // Load recurssion on a separate thread.
          setTimeout(loadChildren(i), 2);
        }
      });
    };

    /**
     * Expands the node.
     */
    TreeNode.prototype.expand = function(state) {
      if (state) {
        this.link.removeClass('collapsed').addClass('expanded');
        this.span.removeClass('collapsed').addClass('expanded');
        this.childlist.show('fast');

        // If this node is checked as including children, go through and select
        // all of it's children.
        if (!params.deepLoad && this.checked && this.include_children) {
          this.include_children = false;
          this.selectChildren(true);
        }
      }
      // Only collapse if they can open it back up.
      else if (this.span.length > 0) {
        this.link.removeClass('expanded').addClass('collapsed');
        this.span.removeClass('expanded').addClass('collapsed');
        this.childlist.hide('fast');
      }

      // If the state is expand, but the children have not been loaded.
      if (state && !this.isLoaded()) {

        // If there are no children, then we need to load them.
        this.loadNode(function(node) {
          if (node.checked) {
            node.selectChildren(node.checked);
          }
          node.expand(true);
        });
      }
    };

    /**
     * Selects all children of this node.
     *
     * @param {boolean} state The state of the selection or array of defaults.
     * @param {function} done Called when we are done selecting.
     */
    TreeNode.prototype.selectChildren = function(state, done, child) {

      // See if the state is a boolean.
      var defaults = (typeof state == 'object');

      // Create a function to call when we are done selecting.
      var doneSelecting = function() {
        if (!child) {

          // If they provided a selected parameter.
          if (params.selected) {
            params.selected(this, true);
          }

          // Say that we are done.
          if (done) {

            done.call(this);
          }
        }
      };

      if (params.deepLoad) {

        // Load all nodes underneath this node.
        this.loadAll(function() {

          // Set this node not busy.
          this.setBusy(false, busyselecting);

          // We are done selecting.
          doneSelecting.call(this);

        }, function(node) {

          var val = state;
          if (defaults) {
            val = state.hasOwnProperty(node.value);
            val |= state.hasOwnProperty(node.id);
          }

          // Select this node.
          node.select(val);
        });
      }
      else {

        // Select the current node.
        this.select(state);
        var name = params.inputName + '-' + this.value;
        $('input[name="' + name + '-include-below"]').attr(
          'name',
          name
        );

        // We should load children if the current node is expanded, or the
        // current node is being deselected and possibly has children selected
        // below them.
        if ((this.root === true) ||
            (state === false && !this.include_children) ||
            (this.link !== undefined && this.link[0] !== undefined &&
             this.link[0].className.indexOf('expanded') !== -1)
           ) {
          this.include_children = false;
          this.expand(state);
          var i = this.children.length;
          while (i--) {

            // Select all the children.
            this.children[i].selectChildren(state, done, true);
          }
        }
        else {
          // Flag this noad as including all children below if it has children.
          if (this.has_children > 0 && state) {
            this.include_children = true;
            $('input[name="' + name + '"]').attr(
              'name',
              name + '-include-below'
            );
          }
        }

        // We are done selecting.
        doneSelecting.call(this);
      }
    };

    /**
     * Selects default values of the TreeNode.
     *
     * @param {boolean} defaults Array of defaults.
     * @param {function} done Called when we are done selecting.
     */
    TreeNode.prototype.selectDefaults = function(defaults, done) {

      var defaultsLeft = Object.keys(defaults).length;

      var defaultsQueue = [];
      defaultsQueue.push(this);

      // Loop through nodes depth first to find the defaults.
      while (defaultsLeft > 0 && defaultsQueue.length > 0) {
        var queueItem = defaultsQueue.shift();
        var state = false;

        // Check if the queued item is listed in the defaults.
        if (defaults.hasOwnProperty(queueItem.value)) {
          delete defaults[queueItem.value];
          state = true;
          defaultsLeft--;
        }
        if (defaults.hasOwnProperty(queueItem.id)) {
          delete defaults[queueItem.id];
          state = true;
          defaultsLeft--;
        }

        // Check if the queued item is listed in the defaults and is flagged to
        // include defaults.
        if (defaults.hasOwnProperty(queueItem.value + '-include-below')) {
          delete defaults[queueItem.value + '-include-below'];
          queueItem.include_children = true;
          state = true;
          defaultsLeft--;
        }
        if (defaults.hasOwnProperty(queueItem.id + '-include-below')) {
          delete defaults[queueItem.id + '-include-below'];
          queueItem.include_children = true;
          state = true;
          defaultsLeft--;
        }

        // Select the queued item.
        queueItem.select(state);

        // Set the input name to the correct value.
        var name = params.inputName + '-' + queueItem.value;
        $('input[name="' + name + '-include-below"]').attr('name', name);
        if (!queueItem.root && state && queueItem.include_children) {
          $('input[name="' + name + '"]').attr('name', name + '-include-below');
        }
        else if (defaultsLeft > 0) {
          // Add this node's children to the queue.
          var i = queueItem.children.length;
          while (i--) {
            defaultsQueue.push(queueItem.children[i]);
          }
        }
        else if (queueItem.root && queueItem.include_children) {

          // Select the root node's children.
          queueItem.selectChildren(true);
        }
      }

      // Say this node is now fully selected.
      if (params.selected) {
        params.selected(this, true);
      }

      // Say we are now done.
      if (done) {
        done.call(this);
      }
    };

    /**
     * Sets the checked state for the input field depending on the state.
     *
     * @param {boolean} state
     */
    TreeNode.prototype.setChecked = function(state) {

      // Set the checked state.
      this.checked = state;

      // Set the checked state for this input.
      if (this.input.length > 0) {
        this.input.eq(0)[0].checked = state;
      }

      // Trigger the change event.
      this.input.change();
    };

    /**
     * Selects a node.
     *
     * @param {boolean} state The state of the selection.
     */
    TreeNode.prototype.select = function(state) {

      // Only check this node if it is a selectable input.
      if (!this.input.hasClass('treenode-no-select')) {

        // Convert state to a boolean.
        state = !!state;

        // Select the element unless the state is false and we are on the root
        // element which isn't unselectable.
        if (state || !this.root || (this.showRoot && this.has_children)) {

          // Set the checked state.
          this.setChecked(state);

          // Say that this node is selected.
          if (params.selected) {
            params.selected(this);
          }
        }
      }
    };

    /**
     * Build the treenode element.
     */
    TreeNode.prototype.build_treenode = function() {
      var treenode = $();
      treenode = $(document.createElement(this.root ? 'div' : 'li'));
      treenode.addClass('treenode');
      treenode.addClass(this.odd ? 'odd' : 'even');
      return treenode;
    };

    /**
     * Build the input and return.
     */
    TreeNode.prototype.build_input = function(left) {

      // Only add an input if the input name is defined.
      if (params.inputName) {

        // If this node is excluded or has no roles enabled in the group finder,
        // then add a dummy div tag.
        if ((typeof this.exclude[this.id] !== 'undefined') ||
          (params.inputName == 'group_finder' && !this.data.roles_enabled)) {
          this.input = $(document.createElement('div'));
          this.input.addClass('treenode-no-select');
        }
        else {

          // Create the input element.
          this.input = $(document.createElement('input'));

          // Get the value for this input item.
          var value = this.value || this.id;

          // Create the attributes for this input item.
          this.input.attr({
            'type': 'checkbox',
            'value': value,
            'name': params.inputName + '-' + value,
            'id': 'choice_' + this.id
          }).addClass('treenode-input');

          // Check the input.
          this.setChecked(this.checked);

          // Bind to the click on the input.
          this.input.bind('click', (function(node) {
            return function(event) {

              // Set the checked state based on input.
              node.checked = event.target.checked;

              // Only expand/collapse and select children if auto select
              // children is enabled.
              if (params.autoSelectChildren) {
                // Expand if deep loading. Collapse if unchecked.
                if (!node.checked || params.deepLoad) {
                  node.expand(node.checked);
                }

                // Call the select method.
                node.selectChildren(node.checked);
              }
            };
          })(this));

          // If this is a root item and we are not showing the root item, then
          // just hide the input.
          if (this.root && !params.showRoot) {
            this.input.hide();
          }
        }

        // Set the input left.
        this.input.css('left', left + 'px');
      }
      return this.input;
    };

    /**
     * Creates a node link.
     */
    TreeNode.prototype.build_link = function(element) {
      element.css('cursor', 'pointer').addClass('collapsed');
      element.bind('click', {node: this}, function(event) {
        event.preventDefault();
        event.data.node.expand($(event.target).hasClass('collapsed'));
      });
      return element;
    };

    /**
     * Build the span +/- symbol.
     */
    TreeNode.prototype.build_span = function(left) {

      // If we are showing the root item or we are not root, and we have
      // children, show a +/- symbol.
      if ((!this.root || this.showRoot) && this.has_children) {
        this.span = this.build_link($(document.createElement('span')).attr({
          'class': 'treeselect-expand'
        }));
        this.span.css('left', left + 'px');
      }
      return this.span;
    };

    /**
     * Build the title link.
     */
    TreeNode.prototype.build_title = function(left) {

      // If there is a title, then build it.
      if ((!this.root || this.showRoot) && this.title) {

        // Create a node link.
        this.nodeLink = $(document.createElement('a')).attr({
          'class': 'treeselect-title',
          'href': this.url,
          'target': '_blank'
        }).css('marginLeft', left + 'px').text(this.title);

        // If this node has children, then it should be a link.
        if (this.has_children) {
          this.link = this.build_link(this.nodeLink.clone());
        }
        else {
          this.link = $(document.createElement('div')).attr({
            'class': 'treeselect-title'
          }).css('marginLeft', left + 'px').text(this.title);
        }
      }

      // Return the link.
      return this.link;
    };

    /**
     * Build the children.
     */
    TreeNode.prototype.build_children = function(done) {

      // Create the childlist element.
      this.childlist = $();

      // If this node has children.
      if (this.children.length > 0) {

        // Create the child listItems.
        this.childlist = $(document.createElement('ul'));

        // Set the odd state.
        var odd = this.odd;

        // Get the number of children.
        var numChildren = this.children.length;

        // Function to append children.
        var appendChildren = function(treenode, index) {
          return function() {

            // Add the child tree to the listItems.
            treenode.children[index].build(function(child) {

              // Decrement the number of children loaded.
              numChildren--;

              // Append the child to the listItems.
              treenode.childlist.append(child.display);

              // If there are no more chlidren, then say we are done.
              if (!numChildren) {
                done.call(treenode, treenode.childlist);
              }
            });
          };
        };

        // Now if there are children, iterate and build them.
        for (var i in this.children) {

          // Make sure the child is a valid object in the listItems.
          if (this.children.hasOwnProperty(i)) {

            // Set the child.
            var child = this.children[i];

            // Alternate the odd state.
            odd = !odd;

            // Get the checked value.
            var isChecked = this.checked;
            if (child.hasOwnProperty('checked')) {
              isChecked = child.checked;
            }

            // Create a new TreeNode for this child.
            this.children[i] = new TreeNode($.extend(child, {
              level: this.level + 1,
              odd: odd,
              checked: isChecked,
              exclude: this.exclude
            }));

            // Set timeout to help with recursion.
            setTimeout(appendChildren(this, i), 2);
          }
        }
      }
      else {

        // Call that we are done loading this child.
        done.call(this, this.childlist);
      }
    };

    /**
     * Builds the DOM and the tree for this node.
     */
    TreeNode.prototype.build = function(done) {

      // Keep track of the left margin for each element.
      var left = 5, elem = null;

      // Create the listItems display.
      if (this.display.length === 0) {
        this.display = this.build_treenode();
      }
      else if (this.root) {
        var treenode = this.build_treenode();
        this.display.append(treenode);
        this.display = treenode;
      }

      // Now append the input.
      if ((this.input.length === 0) &&
          (elem = this.build_input(left)) &&
          (elem.length > 0)) {

        // Add the input to the display.
        this.display.append(elem);
        left += params.colwidth;
      }

      // Now create the +/- sign if needed.
      if (this.span.length === 0) {
        this.display.append(this.build_span(left));
        left += params.colwidth;
      }

      // Now append the node title.
      if (this.link.length === 0) {
        this.display.append(this.build_title(left));
      }

      // Called when the node is done building.
      var onDone = function() {

        // See if they wish to alter the build.
        if (params.onbuild) {
          params.onbuild(this);
        }

        // Create a search item.
        this.searchItem = this.display.clone();
        $('.treeselect-expand', this.searchItem).remove();

        // If the search title is not a link, then make it one...
        var searchTitle = $('div.treeselect-title', this.searchItem);
        if (searchTitle.length > 0) {
          searchTitle.replaceWith(this.nodeLink);
        }

        // See if they wish to hook into the postbuild process.
        if (params.postbuild) {
          params.postbuild(this);
        }

        // Check if this node is excluded, and hide if so.
        if (typeof this.exclude[this.id] !== 'undefined') {
          if ($('.treenode-input', this.display).length === 0) {
            this.display.hide();
          }
        }

        // If they wish to know when we are done building.
        if (done) {
          done.call(this, this);
        }
      };

      // Append the children.
      if (this.childlist.length === 0) {
        this.build_children(function(children) {
          if (children.length > 0) {
            this.display.append(children);
          }
          onDone.call(this);
        });
      }
      else {
        onDone.call(this);
      }
    };

    /**
     * Returns the selectAll text if that applies to this node.
     */
    TreeNode.prototype.getSelectAll = function() {
      if (this.root && this.selectAll) {
        return this.selectAllText;
      }
      return false;
    };

    /**
     * Search this node for matching text.
     *
     * @param {string} text The text to search for.
     * @param {function} callback Called with the results of this search.
     */
    TreeNode.prototype.search = function(text, callback) {
      // If no text was provided, then just return the root children.
      if (!text) {
        if (callback) {
          callback(this.children, false);
        }
      }
      else {

        // Initialize our results.
        var results = {};

        // Convert the text to lowercase.
        text = text.toLowerCase();

        // See if they provided a search endpoint.
        if (params.searcher) {

          // Call the searcher for the new nodes.
          params.searcher(this, text, function(nodes, getNode) {

            // Get the number of nodes.
            var numNodes = Object.keys(nodes).length;

            // If no nodes were returned then return nothing.
            if (numNodes === 0) {
              callback(results, true);
            }

            // Called when the tree node is built.
            var onBuilt = function(id) {

              // Return the method to call when the node is built.
              return function(treenode) {

                // Decrement the counter.
                numNodes--;

                // Add the node to the results.
                results[id] = treenode;

                // If no more nodes are loading, then callback.
                if (!numNodes) {

                  // Callback with the search results.
                  callback(results, true);
                }
              };
            };

            // Iterate through all the nodes.
            for (var id in nodes) {

              // Set the treenode.
              var treenode = new TreeNode(getNode ? getNode(nodes[id]) : nodes[id]);

              // Say this node is loaded.
              treenode.nodeloaded = true;

              // Add to the loaded nodes array.
              loadedNodes[treenode.id] = treenode.id;

              // Build the node.
              treenode.build(onBuilt(id));
            }
          });
        }
        else {

          // Load all nodes.
          this.loadAll(function(node) {

            // Callback with the results of this search.
            if (callback) {
              callback(results, true);
            }
          }, function(node) {

            // If we are not the root node, and the text matches the title.
            if (!node.root && node.title.toLowerCase().search(text) !== -1) {

              // Add this to our search results.
              results[node.id] = node;
            }
          }, true);
        }
      }
    };

    // Iterate through each instance.
    return $(this).each(function() {

      // Get the tree node parameters.
      var treeParams = $.extend(params, {display: $(this)});

      // Create a root tree node and load it.
      var root = this.treenode = new TreeNode(treeParams, true);

      // Add a select all link.
      var selectAll = root.getSelectAll();
      if (selectAll !== false && !root.showRoot) {

        // See if the select all button should be checked.
        var checked = false;
        var default_value = params.default_value;
        if (default_value.hasOwnProperty(root.value + '-include-below')) {
          checked = true;
        }

        // Create an input element.
        var inputElement = $(document.createElement('input')).attr({
          'type': 'checkbox'
        });

        // Set the checked state.
        inputElement.eq(0)[0].checked = checked;

        // Bind to the click event.
        inputElement.bind('click', function(event) {
          root.selectChildren(event.target.checked);
        });

        // Add the input item to the root.
        root.display.append(inputElement);

        // If they provided select all text, add it here.
        if (selectAll) {
          var span = $(document.createElement('span')).attr({
            'class': 'treeselect-select-all'
          }).html(selectAll);
          root.display.append(span);
        }
      }

      // Create a loading span.
      var initBusy = $(document.createElement('span')).addClass('treebusy');
      root.display.append(initBusy.css('display', 'block'));

      // Called when the root node is done loading.
      var doneLoading = function() {

        // Remove the init busy cursor.
        initBusy.remove();

        // Call the treeloaded params.
        if (params.treeloaded) {
          params.treeloaded(this);
        }
      };

      // Load the node.
      root.loadNode(function(node) {

        // Check the length of children in this node.
        if (node.children.length === 0) {

          // If the root node does not have any children, then hide.
          node.display.hide();
        }

        // Expand this root node.
        node.expand(true);

        // Select this node based on the default value.
        node.select(node.checked);

        // Set the defaults for all the children.
        var defaults = node.checked;
        if (!jQuery.isEmptyObject(params.default_value)) {
          defaults = params.default_value;
        }

        // If there are defaults, then select the children with them.
        if (defaults) {

          // Select the children based on the defaults.
          if (params.deepLoad) {
            node.selectChildren(defaults, function() {
              doneLoading.call(node);
            });
          }
          else {
            // When not deep loading, use selectDefaults to search for defaults
            // using breadth first check.
            node.selectDefaults(defaults, function() {
              doneLoading.call(node);
            });
          }
        }
        else {
          doneLoading.call(node);
        }
      });

      // If the root element doesn't have children, then hide the treeselect.
      if (!root.has_children) {
        this.parentElement.style.display = 'none';
      }
    });
  };
})(jQuery);
