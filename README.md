# Publish Times

This is a very simple console application that generates a list of times to publish news articles within a randomized 2-3 hour delay within a 30-minute window to avoid conflicts. This keeps thing flowing at an organic and slow pace.

It is not recommended for use with hot topics. Instead, you should focus on overlooked foreign affairs, local news or op-eds. Of course, this is just covering general news.

## Exporting

Once it generates a list of times, you can retry or export it as a JSON format. If you export, you're requested to select from a number of topics or default to none if no selection is made. Upon export, you can start over or exit.

Choice selection is based on the Y/N keys. ``Enter`` works the same as ``Y`` while pressing any other key is the equivalent of ``N``. You can make mistakes, but it expects failure.

An optional ``config.toml`` file allows for further customization of file name, directory, and topics.

### Example

```json
[
  {
    "topic": "Games",
    "date": "3/16/2025",
    "times": [
      "11:41 AM",
      "2:05 PM",
      "5:05 PM",
      "8:18 PM",
      "11:02 PM"
    ]
  }
]
```

## Background

A while back, I [found a tool](https://schedule.lemmings.world) to schedule articles on Lemmy. I've been posting within a few hours apart at random minutes and I wanted to something decide that for me. I had AI write the base algorithm, everything else is my own touches.

This tool was originally intended with a simple purpose: suggest some publishing times. However, when customable topics were added, it became more versatile. It can be used with anything that can be scheduled to publish, from news, art, or anything else.

## License

I hereby waive this project under the public domain - see [UNLICENSE](UNLICENSE) for details.
