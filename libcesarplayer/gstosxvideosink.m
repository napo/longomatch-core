/* GStreamer
 * OSX video sink
 * Copyright (C) 2004-6 Zaheer Abbas Merali <zaheerabbas at merali dot org>
 * Copyright (C) 2007,2008,2009 Pioneers of the Inevitable <songbird@songbirdnest.com>
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
 * Boston, MA 02110-1301, USA.
 *
 * The development of this code was made possible due to the involvement of
 * Pioneers of the Inevitable, the creators of the Songbird Music player.
 * 
 */

#import <QuartzCore/CATransaction.h>
#include <gst/video/video.h>
#include <gst/interfaces/xoverlay.h>
#include <gst/interfaces/navigation.h>

#include "gstosxvideosink.h"
#include <unistd.h>

#define INIT_POOL \
  NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init]
#define RELEASE_POOL [pool release]

GST_DEBUG_CATEGORY (gst_debug_lgm_osx_video_sink);
#define GST_CAT_DEFAULT gst_debug_lgm_osx_video_sink

static void gst_lgm_osx_video_sink_init_interfaces (GType type);

GST_BOILERPLATE_FULL (GstLgmOSXVideoSink, gst_lgm_osx_video_sink, GstVideoSink,
    GST_TYPE_VIDEO_SINK, gst_lgm_osx_video_sink_init_interfaces);

static GstStaticPadTemplate gst_lgm_osx_video_sink_sink_template_factory =
GST_STATIC_PAD_TEMPLATE ("sink",
    GST_PAD_SINK,
    GST_PAD_ALWAYS,
    GST_STATIC_CAPS ("video/x-raw-yuv, "
        "framerate = (fraction) [ 0, MAX ], "
        "width = (int) [ 1, MAX ], "
        "height = (int) [ 1, MAX ], "
       "format = (fourcc) Y42B")
    );

enum
{
  ARG_0,
  ARG_FORCE_PAR,
};


@implementation GstNSView
-(id) init
{
  INIT_POOL;

  self = [super initWithFrame: [parent_view bounds]];
  layer = [[CALayer layer] retain];
  sublayer = [[CALayer layer] retain];

  [parent_view addSubview:self];
  [self setAutoresizingMask:NSViewWidthSizable|NSViewHeightSizable];
  [self setLayer:self->layer];
  [self setWantsLayer:YES];
  layer.backgroundColor = [NSColor blackColor].CGColor;
  sublayer.backgroundColor = [NSColor redColor].CGColor;
  sublayer.contentsGravity = kCAGravityResize;
  [layer addSublayer:sublayer];
  [[NSNotificationCenter defaultCenter] addObserver:self
    selector:@selector(viewResized:) name:NSViewFrameDidChangeNotification
    object:self];

  RELEASE_POOL;
  return self;
}

-(gboolean) render: (GstBuffer *) buffer
{
  NSImage *image = nil;
  CVPixelBufferRef cv_buf = nil;
  CVReturn cv_ret;
  gboolean ret = FALSE;
  const size_t num_planes = 3;
  void *data;
  void *p_ptr[num_planes];
  size_t p_widths[num_planes];
  size_t p_heights[num_planes];
  size_t p_bytes_per_row[num_planes];
  gint width, height, i;
  GstVideoFormat format = GST_VIDEO_FORMAT_Y42B;
  CIImage *ciImage;
  NSCIImageRep *imageRep;

  if (buffer == NULL) {
    return TRUE;
  }

  INIT_POOL;

  width = GST_VIDEO_SINK_WIDTH (sink);
  height = GST_VIDEO_SINK_HEIGHT (sink);
  data = GST_BUFFER_DATA (buffer);

  for (i = 0; i < num_planes; i++) {
    p_ptr[i] = data + gst_video_format_get_component_offset (format, i, width, height);
    p_bytes_per_row[i] = gst_video_format_get_row_stride (format, i, width);
    p_widths[i] = gst_video_format_get_component_width (format, 0, width);
    p_heights[i] = gst_video_format_get_component_height (format, i, height);
    g_print ("p:%p bpr:%d width:%d height:%d \n",
        p_ptr[i], p_bytes_per_row[i], p_widths[i], p_heights[i]);
  }
  g_print ("P1 %d P2 %d\n", p_ptr[1] - p_ptr[0], p_ptr[2] - p_ptr[1]);
  g_print ("Size %d\n", gst_video_format_get_size (format, width, height));


  cv_ret = CVPixelBufferCreateWithPlanarBytes (NULL,
      width, height, kCVPixelFormatType_422YpCbCr8,
      GST_BUFFER_DATA (buffer), gst_video_format_get_size (format, width, height),
      num_planes, p_ptr, p_widths, p_heights, p_bytes_per_row,
      NULL, NULL, NULL, &cv_buf);
  if (cv_ret != kCVReturnSuccess) {
      goto cv_error;
  }
  ciImage = [[CIImage alloc] initWithCVImageBuffer:cv_buf];
  imageRep = [[NSCIImageRep alloc] initWithCIImage:ciImage];
  NSSize size = [imageRep size];
  g_print ("Size of %f, %f\n", size.width, size.height);
  image = [[NSImage alloc] initWithSize:[imageRep size]];
  [image addRepresentation:imageRep];

  [CATransaction begin];
  {
    [CATransaction setDisableActions:YES];
    [sublayer setContents:image];
    [sublayer setFrame:CGRectMake(0, 0, width, height)];
  }
  [CATransaction commit];

  //[imageRep release];
  //[ciImage release];
  //[image release];
  //CVBufferRelease (cv_buf);

  ret = TRUE;

exit:
  RELEASE_POOL;
  return ret;

cv_error:
  GST_ERROR ("Error creating pixel buffer");
  goto exit;
}

- (void)viewResized:(NSNotification *)notification
{
  INIT_POOL;
  GST_LOG ("view was resized");
  gst_x_overlay_expose (GST_X_OVERLAY (sink));
  RELEASE_POOL;
}

- (BOOL)acceptsFirstResponder {
    return YES;
}

-(void) removeView
{
  INIT_POOL;
  [super removeFromSuperview];
  [super release];
  RELEASE_POOL;
}

-(void) dealloc
{
  INIT_POOL;
  [[NSNotificationCenter defaultCenter] removeObserver:self];
  [layer release];
  [sublayer release];
  [super dealloc];
  RELEASE_POOL;
}
@end

static gboolean
gst_lgm_osx_video_sink_setcaps (GstBaseSink * bsink, GstCaps * caps)
{
  GstLgmOSXVideoSink *osxvideosink;
  GstStructure *structure;
  gboolean res, result = FALSE;
  gint video_width, video_height;

  osxvideosink = GST_LGM_OSX_VIDEO_SINK (bsink);

  GST_DEBUG_OBJECT (osxvideosink, "caps: %" GST_PTR_FORMAT, caps);

  structure = gst_caps_get_structure (caps, 0);
  res = gst_structure_get_int (structure, "width", &video_width);
  res &= gst_structure_get_int (structure, "height", &video_height);

  if (!res) {
    goto beach;
  }

  GST_DEBUG_OBJECT (osxvideosink, "our format is: %dx%d video",
      video_width, video_height);

  GST_VIDEO_SINK_WIDTH (osxvideosink) = video_width;
  GST_VIDEO_SINK_HEIGHT (osxvideosink) = video_height;

  result = TRUE;

beach:
  return result;

}

static GstStateChangeReturn
gst_lgm_osx_video_sink_change_state (GstElement * element,
    GstStateChange transition)
{
  GstLgmOSXVideoSink *osxvideosink;
  GstStateChangeReturn ret;

  osxvideosink = GST_LGM_OSX_VIDEO_SINK (element);

  switch (transition) {
    case GST_STATE_CHANGE_READY_TO_PAUSED:
      /* Creating our window and our image */
      GST_VIDEO_SINK_WIDTH (osxvideosink) = 320;
      GST_VIDEO_SINK_HEIGHT (osxvideosink) = 240;
      GST_INFO_OBJECT (osxvideosink, "emitting prepare-xwindow-id");
      gst_x_overlay_prepare_xwindow_id (GST_X_OVERLAY (osxvideosink));
      break;
    default:
      break;
  }
  GST_DEBUG_OBJECT (osxvideosink, "%s => %s",
        gst_element_state_get_name(GST_STATE_TRANSITION_CURRENT (transition)),
        gst_element_state_get_name(GST_STATE_TRANSITION_NEXT (transition)));

  ret = (GST_ELEMENT_CLASS (parent_class))->change_state (element, transition);

  switch (transition) {
    case GST_STATE_CHANGE_PAUSED_TO_READY: {
       if (osxvideosink->view) {
           [osxvideosink->view removeView];
       }
      break;
    }
    default:
      break;
  }
  return ret;
}

static GstFlowReturn
gst_lgm_osx_video_sink_show_frame (GstBaseSink * bsink, GstBuffer * buf)
{
  GstLgmOSXVideoSink *osxvideosink;
  INIT_POOL;

  osxvideosink = GST_LGM_OSX_VIDEO_SINK (bsink);

  GST_DEBUG ("show_frame");
  [osxvideosink->view render: buf];
  RELEASE_POOL;
  return GST_FLOW_OK;
}

/* =========================================== */
/*                                             */
/*              Init & Class init              */
/*                                             */
/* =========================================== */

static void
gst_lgm_osx_video_sink_set_property (GObject * object, guint prop_id,
    const GValue * value, GParamSpec * pspec)
{
  GstLgmOSXVideoSink *osxvideosink;

  g_return_if_fail (GST_IS_LGM_OSX_VIDEO_SINK (object));

  osxvideosink = GST_LGM_OSX_VIDEO_SINK (object);

  switch (prop_id) {
    case ARG_FORCE_PAR:
      osxvideosink->keep_par = g_value_get_boolean(value);
      if (osxvideosink->view) {
        osxvideosink->view->keep_par = osxvideosink->keep_par;
      }
      break;
    default:
      G_OBJECT_WARN_INVALID_PROPERTY_ID (object, prop_id, pspec);
      break;
  }
}

static void
gst_lgm_osx_video_sink_get_property (GObject * object, guint prop_id,
    GValue * value, GParamSpec * pspec)
{
  GstLgmOSXVideoSink *osxvideosink;

  g_return_if_fail (GST_IS_LGM_OSX_VIDEO_SINK (object));

  osxvideosink = GST_LGM_OSX_VIDEO_SINK (object);

  switch (prop_id) {
    case ARG_FORCE_PAR:
      g_value_set_boolean (value, osxvideosink->keep_par);
      break;
    default:
      G_OBJECT_WARN_INVALID_PROPERTY_ID (object, prop_id, pspec);
      break;
  }
}

static void
gst_lgm_osx_video_sink_init (GstLgmOSXVideoSink * sink,
    GstLgmOSXVideoSinkClass *sinkclass)
{
  sink->view = NULL;
  sink->keep_par = FALSE;
}

static void
gst_lgm_osx_video_sink_base_init (gpointer g_class)
{
  GstElementClass *element_class = GST_ELEMENT_CLASS (g_class);

  gst_element_class_set_details_simple (element_class, "OSX Video sink",
      "Sink/Video", "OSX native videosink",
      "Zaheer Abbas Merali <zaheerabbas at merali dot org>");

  gst_element_class_add_static_pad_template (element_class,
      &gst_lgm_osx_video_sink_sink_template_factory);
}

static void
gst_lgm_osx_video_sink_finalize (GObject *object)
{
  GstLgmOSXVideoSink *osxvideosink = GST_LGM_OSX_VIDEO_SINK (object);

  if (osxvideosink->view) {
    [osxvideosink->view removeView];
    osxvideosink->view = NULL;;
  }

  G_OBJECT_CLASS (parent_class)->finalize (object);
}

static void
gst_lgm_osx_video_sink_class_init (GstLgmOSXVideoSinkClass * klass)
{
  GObjectClass *gobject_class;
  GstElementClass *gstelement_class;
  GstBaseSinkClass *gstbasesink_class;

  gobject_class = (GObjectClass *) klass;
  gstelement_class = (GstElementClass *) klass;
  gstbasesink_class = (GstBaseSinkClass *) klass;

  parent_class = g_type_class_ref (GST_TYPE_VIDEO_SINK);

  gobject_class->set_property = gst_lgm_osx_video_sink_set_property;
  gobject_class->get_property = gst_lgm_osx_video_sink_get_property;
  gobject_class->finalize = gst_lgm_osx_video_sink_finalize;

  gstbasesink_class->set_caps = gst_lgm_osx_video_sink_setcaps;
  gstbasesink_class->preroll = gst_lgm_osx_video_sink_show_frame;
  gstbasesink_class->render = gst_lgm_osx_video_sink_show_frame;
  gstelement_class->change_state = gst_lgm_osx_video_sink_change_state;

  /**
   * GstLgmOSXVideoSink:force-aspect-ratio
   *
   * When enabled, scaling will respect original aspect ratio.
   *
   **/

  g_object_class_install_property (gobject_class, ARG_FORCE_PAR,
      g_param_spec_boolean ("force-aspect-ratio", "force aspect ration",
          "When enabled, scaling will respect original aspect ration",
          TRUE, G_PARAM_READWRITE | G_PARAM_STATIC_STRINGS));

  GST_DEBUG_CATEGORY_INIT (gst_debug_lgm_osx_video_sink, "lgmosxvideosink", 0,
      "osxvideosink element");
}

static void
gst_lgm_osx_video_sink_set_window_handle (GstXOverlay * overlay, guintptr handle_id)
{
  GstLgmOSXVideoSink *osxvideosink = GST_LGM_OSX_VIDEO_SINK (overlay);
  GstVideoSink *sink = GST_VIDEO_SINK (overlay);
  NSView *view = (NSView *) handle_id;

  NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];

  if (osxvideosink->view) {
    if (view == osxvideosink->view->parent_view)
      return;
    else
      [osxvideosink->view removeView];
  }

  osxvideosink->view = [GstNSView alloc];
  osxvideosink->view->parent_view = view;
  osxvideosink->view->sink = sink;
  osxvideosink->view->keep_par = osxvideosink->keep_par;
  [osxvideosink->view performSelectorOnMainThread:@selector(init)
      withObject:(id)nil waitUntilDone:NO];

  [pool release];
}

static void
gst_lgm_osx_video_sink_xoverlay_init (GstXOverlayClass * iface)
{
  iface->set_window_handle = gst_lgm_osx_video_sink_set_window_handle;
  iface->expose = NULL;
  iface->handle_events = NULL;
}

static gboolean
gst_lgm_osx_video_sink_interface_supported (GstImplementsInterface * iface, GType type)
{
  if (type == GST_TYPE_X_OVERLAY)
    return TRUE;
  else
    return FALSE;
}

static void
gst_lgm_osx_video_sink_interface_init (GstImplementsInterfaceClass * klass)
{
  klass->supported = gst_lgm_osx_video_sink_interface_supported;
}

static void
gst_lgm_osx_video_sink_init_interfaces (GType type)
{
  static const GInterfaceInfo iface_info = {
    (GInterfaceInitFunc) gst_lgm_osx_video_sink_interface_init,
    NULL,
    NULL,
  };
  static const GInterfaceInfo overlay_info = {
    (GInterfaceInitFunc) gst_lgm_osx_video_sink_xoverlay_init,
    NULL,
    NULL,
  };

  g_type_add_interface_static (type,
      GST_TYPE_IMPLEMENTS_INTERFACE, &iface_info);
  g_type_add_interface_static (type, GST_TYPE_X_OVERLAY, &overlay_info);
}

static gboolean
plugin_init (GstPlugin * plugin)
{
  if (!gst_element_register (plugin, "lgmosxvideosink",
          GST_RANK_PRIMARY + 1, GST_TYPE_LGM_OSX_VIDEO_SINK))
    return FALSE;

  return TRUE;
}

GST_PLUGIN_DEFINE2 (GST_VERSION_MAJOR,
    GST_VERSION_MINOR,
    osxvideo,
    "OSX native video output plugin",
    plugin_init, VERSION, "LGPL", "longomatch", "http://longomatch.org")
